using ElectronicShopper.DataAccess.StoredProcedures.Inventory;
using ElectronicShopper.DataAccess.StoredProcedures.Product;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Models;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ProductModel = ElectronicShopper.Library.Models.ProductModel;

namespace ElectronicShopper.DataAccess.Data;

public class ProductData : IProductData
{
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
        ILogger<ProductData> logger)
    {
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
        _imageSettings = imageSettings.Value;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task Create(ProductInsertModel product)
    {
        await _productValidator.ValidateAndThrowAsync(product);

        var category = await _categoryData.Get((int)product.CategoryId!);
        if (category is null)
            throw new DatabaseException("Category is not present in the database");

        if (product.Template?.Id != null)
            await CreateTemplate(product.Template);

        var sp = _mapper.Map<ProductInsertStoredProcedure>(product);

        try
        {
            _sql.StartTransaction(_connectionString);
            var id = await _sql.SaveData<ProductInsertStoredProcedure, int>(sp);
            product.Id = id;
            _logger.LogInformation("Created product {Name} with Id {Id}", product.ProductName, product.Id);


            var p = new ProductModel { Id = id, Inventory = product.Inventory, ProductName = product.ProductName };
            foreach (var image in product.Images)
            {
                await CreateImageWithoutTransaction(p, image);
            }

            await CreateInventoryWithoutTransaction((int)p.Id!, p.Inventory);
            _sql.CommitTransaction();
        }
        catch (Exception)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }


    private async Task CreateInventoryWithoutTransaction(int productId, InventoryModel inventory)
    {
        var sp = _mapper.Map<InventoryInsertStoredProcedure>(inventory);
        sp.ProductId = productId;
        var id = await _sql.SaveData<InventoryInsertStoredProcedure, int>(sp);
        inventory.Id = id;
        _logger.LogInformation("Created inventory for product {ProductId} with Id {Id}", productId, inventory.Id);
    }


    /// <summary>
    /// Add product to the database. Requires opened transaction.
    /// </summary>
    /// <param name="product">Product to which image will be added.</param>
    /// <param name="image">Image to add.</param>
    private async Task CreateImageWithoutTransaction(ProductModel product, MemoryImageModel image)
    {
        await _imageValidator.ValidateAndThrowAsync(image);
        var folderPath = $"{_imageSettings.Products}/{product.ProductName}";
        var filePath = $"{folderPath}/{image.Name}{image.Extension}";
        var sp = _mapper.Map<ProductImageInsertStoredProcedure>(product);
        _mapper.Map(image, sp);
        sp.Path = filePath;

        folderPath = $"{_imageSettings.BasePath}{folderPath}";
        filePath = $"{_imageSettings.BasePath}{filePath}";

        var id = await _sql.SaveData<ProductImageInsertStoredProcedure, int>(sp);
        if (_fileSystem.Exists(folderPath) == false)
            _fileSystem.CreateDirectory(folderPath);
        await _fileSystem.Save(filePath, image.Stream);
        image.Id = id;
        _logger.LogInformation("Created image {Name} with Id {Id}", image.Name, image.Id);
    }

    public async Task CreateTemplate(ProductTemplateModel template)
    {
        await _templateValidator.ValidateAndThrowAsync(template);
        var sp = _mapper.Map<ProductTemplateInsertStoredProcedure>(template);

        try
        {
            _sql.StartTransaction(_connectionString);
            var id = await _sql.SaveData<ProductTemplateInsertStoredProcedure, int>(sp);
            _sql.CommitTransaction();
            template.Id = id;
            _logger.LogInformation("Created template {Name} with Id {Id}", template.Name, template.Id);
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task CreateImage(ProductModel product, MemoryImageModel image)
    {
        try
        {
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

        if (result is null)
            return null;

        var output = _mapper.Map<ProductModel?>(result);


        output!.Category = (await _categoryData.Get(result.CategoryId))!;
        output.Images = await GetProductImages(output);
        await SetInventory(output);
        return output;
    }

    public async Task<List<ProductModel>> GetAll()
    {
        IEnumerable<ProductDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result =
                await _sql.LoadData<ProductGetAllStoredProcedure, ProductDbModel>(new ProductGetAllStoredProcedure());
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        var output = _mapper.Map<List<ProductModel>>(result);

        foreach (var item in output)
        {
            var category = await _categoryData.Get((int)item.Category.Id!);
            if (category is null)
                throw new NullReferenceException(nameof(category));

            item.Category = category;
            item.Images = await GetProductImages(item);
            await SetInventory(item);
        }

        return output;
    }

    public async Task<ProductTemplateModel?> GetTemplate(int templateId)
    {
        var sp = new ProductTemplateGetByIdStoredProcedure { Id = templateId };
        ProductTemplateDbModel? result;

        try
        {
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

    public async Task<List<ProductTemplateModel>> GetAllTemplates()
    {
        IEnumerable<ProductTemplateDbModel> result;
        try
        {
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

        var output = _mapper.Map<List<ProductTemplateModel>>(result);

        return output;
    }

    public async Task<List<ProductImageModel>> GetProductImages(ProductModel product)
    {
        var sp = _mapper.Map<ProductGetProductImagesStoredProcedure>(product);
        IEnumerable<ProductImageDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result = await _sql.LoadData<ProductGetProductImagesStoredProcedure, ProductImageDbModel>(sp);
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

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
}