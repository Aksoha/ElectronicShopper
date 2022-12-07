using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ElectronicShopper.Library.Models.Internal;
using ElectronicShopper.Library.StoredProcedures.Inventory;
using ElectronicShopper.Library.StoredProcedures.Product;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ElectronicShopper.Library.Data;

public class ProductData : IProductData
{
    /// <summary>
    ///     Key value used to cache <see cref="ProductModel" />.
    /// </summary>
    private const string CacheName = "ProductData";

    private readonly IMemoryCache _cache;

    /// <summary>
    ///     Time after which cache will be removed.
    /// </summary>
    private readonly TimeSpan _cacheTime = TimeSpan.FromDays(1);

    private readonly ICategoryData _categoryData;
    private readonly string _connectionString;
    private readonly IFileSystem _fileSystem;
    private readonly ImageStorageSettings _imageSettings;
    private readonly IValidator<MemoryImageModel> _imageValidator;
    private readonly ILogger<ProductData> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductInsertModel> _productValidator;
    private readonly ISqlDataAccess _sql;
    private readonly IValidator<ProductTemplateModel> _templateValidator;

    public ProductData(ISqlDataAccess sql, IMapper mapper, ICategoryData categoryData,
        IOptionsSnapshot<ConnectionStringSettings> settings, IOptionsSnapshot<ImageStorageSettings> imageSettings
        , IFileSystem fileSystem, IValidator<ProductInsertModel> productValidator,
        IValidator<MemoryImageModel> imageValidator,
        IValidator<ProductTemplateModel> templateValidator,
        ILogger<ProductData> logger, IMemoryCache cache)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(templateValidator);
        ArgumentNullException.ThrowIfNull(imageValidator);
        ArgumentNullException.ThrowIfNull(productValidator);
        ArgumentNullException.ThrowIfNull(fileSystem);
        ArgumentNullException.ThrowIfNull(imageSettings);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(categoryData);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sql);

        _sql = sql;
        _mapper = mapper;
        _categoryData = categoryData;
        _fileSystem = fileSystem;
        _productValidator = productValidator;
        _imageValidator = imageValidator;
        _templateValidator = templateValidator;
        _logger = logger;
        _cache = cache;
        _imageSettings = imageSettings.Value;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task Create(ProductInsertModel product)
    {
        // validate if model passes all criteria to be inserted
        await _productValidator.ValidateAndThrowAsync(product);
        var category = await _categoryData.Get((int)product.CategoryId!);
        if (category is null)
            throw new DatabaseException("Category is not present in the database");


        try
        {
            var sp = _mapper.Map<ProductInsertStoredProcedure>(product);
            _sql.StartTransaction(_connectionString);
            // insert into ProductTemplate if template was provided and was not in the database
            if (product.Template is not null && product.Template.Id is null)
            {
                if (product.Template.Id is not null)
                {
                    var templateDoesNotExist = await GetTemplate((int)product.Template.Id) is null;
                    if (templateDoesNotExist)
                        throw new DatabaseException(@$"Provided product template with Id {product.Template.Id} was 
                                        not found in the database");
                }

                await CreateTemplateWithoutTransaction(product.Template);
            }

            // insert into Product table
            var id = await _sql.SaveData<ProductInsertStoredProcedure, int>(sp);
            product.Id = id;
            _logger.LogInformation("Created product {Name} with Id {Id}", product.ProductName, product.Id);


            // insert into ProductImage table
            foreach (var image in product.Images) await CreateImageWithoutTransaction(product.Id, image);

            // insert into Inventory table
            await CreateInventoryWithoutTransaction((int)product.Id, product.Inventory);
            _sql.CommitTransaction();


            // set cache
            var p = await Get(id);
            AddProductToCache(p!);
        }
        catch (Exception)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task CreateTemplate(ProductTemplateModel template)
    {
        try
        {
            _sql.StartTransaction(_connectionString);
            await CreateTemplateWithoutTransaction(template);
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }
        catch (ValidationException)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task CreateImage(ProductModel product, MemoryImageModel image)
    {
        // validate
        await _imageValidator.ValidateAndThrowAsync(image);
        try
        {
            // add to database
            _sql.StartTransaction(_connectionString);
            await CreateImageWithoutTransaction(product, image);
            _sql.CommitTransaction();
        }
        catch (Exception)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task CreateInventory(int productId, InventoryModel inventory)
    {
        try
        {
            // add to database
            _sql.StartTransaction(_connectionString);
            await CreateInventoryWithoutTransaction(productId, inventory);
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task<ProductModel?> Get(int id)
    {
        // try to get product from cache
        var cachedProduct = GetProductFromCache(id);
        if (cachedProduct is not null)
        {
            _logger.LogDebug("Fetched product with Id {Id} and Name {Name} from cache",
                cachedProduct.Id, cachedProduct.ProductName);
            return cachedProduct;
        }


        // try to get product from database
        var sp = new ProductGetStoredProcedure { Id = id };
        ProductDbModel? result;

        try
        {
            _sql.StartTransaction(_connectionString);
            result = (await _sql.LoadData<ProductGetStoredProcedure, ProductDbModel>(sp)).FirstOrDefault();
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        // product not found in cache and database
        if (result is null)
        {
            _logger.LogDebug("Product with Id {Id} was not found", id);
            return null;
        }


        // convert database model and cache product
        var output = _mapper.Map<ProductModel>(result);
        _logger.LogDebug("Fetched category with Id {Id} and Name {Name} from database", id,
            output.ProductName);

        output.Category = (await _categoryData.Get(result.CategoryId))!;
        output.Images = (await GetProductImages(output)).ToList();
        await SetInventory(output);
        AddProductToCache(output);
        return output;
    }

    public async Task<IEnumerable<ProductModel>> GetAll()
    {
        // try to get products from cache
        var cachedProducts = GetProductsFromCache();
        if (cachedProducts is not null)
        {
            _logger.LogDebug("Fetched all products from cache");
            return cachedProducts;
        }


        // try to get products from database
        IEnumerable<ProductDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result =
                await _sql.LoadData<ProductGetAllStoredProcedure, ProductDbModel>(new ProductGetAllStoredProcedure());
            _sql.CommitTransaction();
            _logger.LogDebug("Fetched all products from database");
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        // convert database model and cache
        var output = _mapper.Map<List<ProductModel>>(result);
        foreach (var item in output)
        {
            var category = await _categoryData.Get((int)item.Category.Id!);
            if (category is null)
                throw new UnreachableException("category was not set to instance of an object");

            item.Category = category;
            item.Images = (await GetProductImages(item)).ToList();
            await SetInventory(item);
            AddProductToCache(item);
        }

        return output;
    }

    public async Task<ProductTemplateModel?> GetTemplate(int templateId)
    {
        var sp = new ProductTemplateGetByIdStoredProcedure { Id = templateId };
        ProductTemplateDbModel? result;

        try
        {
            // get template from database
            _sql.StartTransaction(_connectionString);
            result = (await _sql.LoadData<ProductTemplateGetByIdStoredProcedure, ProductTemplateDbModel>(sp))
                .FirstOrDefault();
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        if (result is null)
            return null;

        var output = _mapper.Map<ProductTemplateModel>(result);
        return output;
    }

    public async Task<IEnumerable<ProductTemplateModel>> GetAllTemplates()
    {
        IEnumerable<ProductTemplateDbModel> result;
        try
        {
            //get templates from the database
            _sql.StartTransaction(_connectionString);
            result = await _sql.LoadData<ProductTemplateGetAllStoredProcedure, ProductTemplateDbModel>(
                new ProductTemplateGetAllStoredProcedure());
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        // convert database model
        var output = _mapper.Map<List<ProductTemplateModel>>(result);
        return output;
    }

    public async Task<IEnumerable<ProductImageModel>> GetProductImages(ProductModel product)
    {
        var sp = _mapper.Map<ProductGetProductImagesStoredProcedure>(product);
        IEnumerable<ProductImageDbModel> result;
        try
        {
            // get images from the database
            _sql.StartTransaction(_connectionString);
            result = await _sql.LoadData<ProductGetProductImagesStoredProcedure, ProductImageDbModel>(sp);
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        // convert database model
        var output = _mapper.Map<List<ProductImageModel>>(result);
        return output;
    }

    public async Task UpdateInventory(ProductModel product, InventoryModel inventory)
    {
        ArgumentNullException.ThrowIfNull(product.Id);

        var sp = _mapper.Map<InventoryUpdateStoredProcedure>(inventory);
        _mapper.Map(product, sp);
        try
        {
            // update inventory
            _sql.StartTransaction(_connectionString);
            await _sql.SaveData(sp);
            _sql.CommitTransaction();
            inventory.Id = product.Inventory.Id;
            product.Inventory = inventory;
        }
        catch (SqlException e)
        {
            _sql.RollbackTransaction();
            if (e.Message == "Inventory table does not contain row with given product")
                throw new DatabaseException("Inventory table does not contain row with given product");
            throw;
        }
    }


    /// <summary>
    ///     Created inventory of a product in the database. Requires opened transaction.
    /// </summary>
    /// <param name="productId">Id of a product whom inventory is to be created.</param>
    /// <param name="inventory">Inventory to add.</param>
    private async Task CreateInventoryWithoutTransaction(int productId, InventoryModel inventory)
    {
        var sp = _mapper.Map<InventoryInsertStoredProcedure>(inventory);
        sp.ProductId = productId;
        var id = await _sql.SaveData<InventoryInsertStoredProcedure, int>(sp);
        inventory.Id = id;
        _logger.LogInformation("Created inventory for product {ProductId} with Id {Id}", productId, inventory.Id);
    }


    /// <summary>
    ///     Add product image to the database. Requires opened transaction.
    /// </summary>
    /// <param name="product">Product to which image will be added.</param>
    /// <param name="image">Image to add.</param>
    private async Task CreateImageWithoutTransaction(ProductModel product, MemoryImageModel image)
    {
        // validate if image can be inserted
        await _imageValidator.ValidateAndThrowAsync(image);


        var folderPath = $"{_imageSettings.Products}/{product.ProductName}";
        var filePath = $"{folderPath}/{image.Name}{image.Extension}";


        var sp = _mapper.Map<ProductImageInsertStoredProcedure>(product);
        _mapper.Map(image, sp);
        sp.Path = filePath;

        folderPath = $"{_imageSettings.BasePath}{folderPath}";
        filePath = $"{_imageSettings.BasePath}{filePath}";


        // add image data which contains a path where image is stored to the database
        var id = await _sql.SaveData<ProductImageInsertStoredProcedure, int>(sp);


        // store image
        if (_fileSystem.Exists(folderPath) == false)
            _fileSystem.CreateDirectory(folderPath);
        await _fileSystem.Save(filePath, image.Stream);
        image.Id = id;
        _logger.LogInformation("Created image {Name} with Id {Id}", image.Name, image.Id);
    }

    /// <summary>
    ///     Add product image to the database. Requires opened transaction.
    /// </summary>
    /// <param name="productId">Id of product whose the image will be added.</param>
    /// <param name="image">Image to add.</param>
    private async Task CreateImageWithoutTransaction([DisallowNull] int? productId, MemoryImageModel image)
    {
        var sp = new ProductGetStoredProcedure { Id = (int)productId };
        var result = (await _sql.LoadData<ProductGetStoredProcedure, ProductDbModel>(sp)).FirstOrDefault();
        var product = _mapper.Map<ProductModel>(result);
        if (product is null)
            throw new UnreachableException(@$"Call of this method expect product {productId} 
                    to be in the database but it was not found");
        await CreateImageWithoutTransaction(product, image);
    }

    /// <summary>
    ///     Created product template in the database. Requires opened transaction.
    /// </summary>
    /// <param name="template">Product template to add.</param>
    private async Task CreateTemplateWithoutTransaction(ProductTemplateModel template)
    {
        //validate
        await _templateValidator.ValidateAndThrowAsync(template);
        var sp = _mapper.Map<ProductTemplateInsertStoredProcedure>(template);

        // add to the database
        var id = await _sql.SaveData<ProductTemplateInsertStoredProcedure, int>(sp);
        template.Id = id;
        _logger.LogInformation("Created template {Name} with Id {Id}", template.Name, template.Id);
    }

    /// <summary>
    ///     Sets <see cref="ProductModel.Inventory" /> field of product.
    /// </summary>
    /// <param name="product">Product who's inventory is to be set.</param>
    private async Task SetInventory(ProductModel product)
    {
        var sp = _mapper.Map<InventoryGetByProductIdStoredProcedure>(product);
        _sql.StartTransaction(_connectionString);
        var result =
            (await _sql.LoadData<InventoryGetByProductIdStoredProcedure, InventoryDbModel>(sp)).Single();
        _sql.CommitTransaction();

        product.Inventory = _mapper.Map<InventoryModel>(result);
    }


    /// <summary>
    ///     Creates new collection of items that is stored in the memory.
    /// </summary>
    /// <returns>Newly created collection.</returns>
    private ConcurrentBag<ProductModel> CreateCacheIfItNotExist()
    {
        var cachedProducts = GetProductsFromCache();
        if (cachedProducts is not null) return cachedProducts;

        cachedProducts = new ConcurrentBag<ProductModel>();
        _cache.Set(CacheName, cachedProducts, _cacheTime);
        _logger.LogDebug("Created new product collection with hash {Hash} in cache", cachedProducts.GetHashCode());

        return cachedProducts;
    }


    /// <summary>
    ///     Adds <see cref="ProductModel" /> to cache.
    /// </summary>
    /// <param name="product">Product to be added.</param>
    private void AddProductToCache(ProductModel product)
    {
        var cachedProducts = CreateCacheIfItNotExist();
        if (cachedProducts.Contains(product)) return;

        cachedProducts.Add(product);
        _logger.LogDebug("Added product with Id {Id} to cache", product.Id);
    }


    /// <summary>
    ///     Retrieves <see cref="ProductModel" /> from cache.
    /// </summary>
    /// <param name="id">Id of product to retrieve.</param>
    /// <returns><see cref="ProductModel" /> if object was found, otherwise <see langoword="null" />.</returns>
    private ProductModel? GetProductFromCache(int id)
    {
        var cachedProducts = GetProductsFromCache();
        return cachedProducts?.SingleOrDefault(x => x.Id == id);
    }


    /// <summary>
    ///     Retrieves collection of <see cref="ProductModel" /> from cache.
    /// </summary>
    /// <returns>Collection if cache was set or not expired otherwise <see langoword="null" />.</returns>
    private ConcurrentBag<ProductModel>? GetProductsFromCache()
    {
        return _cache.Get<ConcurrentBag<ProductModel>>(CacheName);
    }
}