using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.Models;

/// <summary>
///     A class used for creating new <see cref="ProductModel" /> in the database.
/// </summary>
public class ProductInsertModel : IDbEntity
{
    /// <inheritdoc cref="ProductDbModel.CategoryId" />
    public int? CategoryId { get; set; }

    /// <inheritdoc cref="ProductDbModel.ProductName" />
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    ///     Template used for creating product.
    /// </summary>
    public ProductTemplateModel? Template { get; set; }


    /// <inheritdoc cref="ProductModel.Images" />
    public List<MemoryImageModel> Images { get; set; } = new();

    /// <inheritdoc cref="ProductModel.Inventory" />
    public InventoryModel Inventory { get; set; } = new();

    /// <inheritdoc cref="ProductModel.Properties" />
    public Dictionary<string, List<string>> Properties { get; set; } = new();

    public int? Id { get; set; }
}