namespace ElectronicShopper.Library.Data;

/// <summary>
///     Provides access to products and inventory in the database.
/// </summary>
public interface IProductData
{
    /// <summary>
    ///     Adds product to the database.
    /// </summary>
    /// <param name="product">Product to add.</param>
    /// <exception cref="FluentValidation.ValidationException">
    ///     Thrown when <paramref name="product" />
    ///     does not pass data validation.
    /// </exception>
    /// <exception cref="DatabaseException">
    ///     Thrown when <see cref="ProductInsertModel.CategoryId" />
    ///     is not associated with category in the database.
    /// </exception>
    Task Create(ProductInsertModel product);

    /// <summary>
    ///     Adds product template to the database.
    /// </summary>
    /// <param name="template">Template to add.</param>
    /// <exception cref="FluentValidation.ValidationException">
    ///     Thrown when <paramref name="template" />
    ///     does not pass data validation.
    /// </exception>
    /// <exception cref="FluentValidation.ValidationException">
    ///     Thrown when <paramref name="template" />
    ///     does not pass data validation.
    /// </exception>
    Task CreateTemplate(ProductTemplateModel template);

    /// <summary>
    ///     Add product image to the database.
    /// </summary>
    /// <param name="product">Product to which image will be added.</param>
    /// <param name="image">Image to add.</param>
    Task CreateImage(ProductModel product, MemoryImageModel image);

    /// <summary>
    ///     Add product inventory to the database.
    /// </summary>
    /// <param name="productId">Id of the product.</param>
    /// <param name="inventory">Inventory to add.</param>
    [Obsolete(@"Use of this method is not needed because right now there can be 
        only one InventoryModel per Product in the database. Product InventoryModel is automatically is created with Create method", true)]
    Task CreateInventory(int productId, InventoryModel inventory);

    /// <summary>
    ///     Retrieve specific product.
    /// </summary>
    /// <param name="id">Id of product.</param>
    Task<ProductModel?> Get(int id);

    /// <summary>
    ///     Retrieve all products.
    /// </summary>
    Task<IEnumerable<ProductModel>> GetAll();

    /// <summary>
    ///     Retrieve product template.
    /// </summary>
    /// <param name="templateId">Id of template.</param>
    Task<ProductTemplateModel?> GetTemplate(int templateId);

    /// <summary>
    ///     Retrieve all templates.
    /// </summary>
    Task<IEnumerable<ProductTemplateModel>> GetAllTemplates();

    /// <summary>
    ///     Retrieve all product images.
    /// </summary>
    /// <param name="product">Product who's images are to be retrieved.</param>
    Task<IEnumerable<ProductImageModel>> GetProductImages(ProductModel product);

    /// <summary>
    ///     Updates existing inventory.
    /// </summary>
    /// <param name="product">Product who's inventory is to be updated.</param>
    /// <param name="inventory">New product inventory.</param>
    /// <exception cref="DatabaseException">
    ///     Thrown when there was no row associated with
    ///     product to update in the database.
    /// </exception>
    Task UpdateInventory(ProductModel product, InventoryModel inventory);
}