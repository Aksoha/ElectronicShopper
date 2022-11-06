using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Settings;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.DataAccess.Data;

public class ProductData : IProductData
{
    private readonly ISqlDataAccess _sql;
    private readonly ICategoryData _categoryData;
    private readonly IMapper _mapper;
    private readonly string _connectionString;

    public ProductData(ISqlDataAccess sql, ICategoryData categoryData, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings)
    {
        _sql = sql;
        _categoryData = categoryData;
        _mapper = mapper;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task<ProductModel?> GetProduct(int id)
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<ProductDbModel, dynamic>("dbo.spProduct_Get", new { Id = id });
        _sql.CommitTransaction();

        var output = _mapper.Map<ProductModel?>(result.SingleOrDefault());
        return output;
    }


    public async Task<List<ProductModel>> GetProducts()
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<ProductDbModel, dynamic>("dbo.spProduct_GetAll", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map<List<ProductModel>>(result);

        foreach (var item in output)
        {
            var category = await _categoryData.GetById(item.Category.Id);
            if (category is null)
                throw new NullReferenceException(nameof(category));

            item.Category = category;
            item.Images = await GetProductImages(item);
        }

        return output;
    }

    public async Task<List<ProductTemplateModel>> GetTemplates()
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<ProductTemplateDbModel, dynamic>("dbo.spProductTemplate_GetAll", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map<List<ProductTemplateModel>>(result);

        return output;
    }

    public async Task<ProductTemplateModel?> GetTemplateById(int id)
    {
        _sql.StartTransaction(_connectionString);
        var result =
            await _sql.LoadData<ProductTemplateModel, dynamic>("dbo.spProductTemplate_GetById", new { Id = id });
        _sql.CommitTransaction();

        var output = _mapper.Map<ProductTemplateModel>(result.FirstOrDefault());
        return output;
    }

    public async Task<List<ProductImageModel>> GetProductImages(ProductModel product)
    {
        _sql.StartTransaction(_connectionString);
        var result =
            await _sql.LoadData<ProductImageDbModel, dynamic>("spProduct_GetProductImages",
                new { ProductId = product.Id });
        _sql.CommitTransaction();

        var output = _mapper.Map<List<ProductImageModel>>(result);

        return output;
    }

    public async Task AddProduct(ProductModel product, ProductTemplateModel template)
    {
        var templateExistsInDb = await GetTemplateById(template.Id) is not null;

        if (templateExistsInDb == false)
            await AddTemplate(template);
        
        var dbItem = _mapper.Map<ProductDbModel>(product);

        _sql.StartTransaction(_connectionString);
        var id = await _sql.SaveData<dynamic, int>("spProduct_Insert",
            new
            {
                dbItem.CategoryId, template.Id, dbItem.RetailPrice, dbItem.ProductName, dbItem.Properties
            });
        _sql.CommitTransaction();

        product.Id = id;
    }

    public async Task AddTemplate(ProductTemplateModel template)
    {
        var dbItem = _mapper.Map<ProductTemplateDbModel>(template);

        _sql.StartTransaction(_connectionString);
        var id = await _sql.SaveData<dynamic, int>("spProductTemplate_Insert", new { dbItem.Properties });
        _sql.CommitTransaction();

        template.Id = id;
    }

    public async Task AddProductImage(ProductModel product, ProductImageModel image)
    {
        var dbItem = _mapper.Map<ProductImageDbModel>(image);
        _sql.StartTransaction(_connectionString);
        var id = await _sql.SaveData<dynamic, int>("spProductImage_Insert",
            new { dbItem.ProductId, dbItem.Path, dbItem.IsPrimary });
        _sql.CommitTransaction();

        image.Id = id;
    }
}