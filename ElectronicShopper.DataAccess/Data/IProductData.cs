using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Data;

public interface IProductData
{
    Task<ProductModel?> GetProduct(int id);
    Task<List<ProductModel>> GetProducts();
    Task<List<ProductTemplateModel>> GetTemplates();
    Task<ProductTemplateModel?> GetTemplateById(int id);
    Task<List<ProductImageModel>> GetProductImages(ProductModel product);
    Task AddProduct(ProductModel product, ProductTemplateModel template);
    Task AddTemplate(ProductTemplateModel template);
    Task AddProductImage(ProductModel product, ProductImageModel image);
}