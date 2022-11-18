namespace ElectronicShopper.Library.Models;

public class ProductModel : IDbEntity
{
    public int? Id { get; set; }
    public CategoryModel Category { get; set; } = new();
    public InventoryModel Inventory { get; set; } = new();
    public string ProductName { get; set; } = string.Empty;
    public List<ProductImageModel> Images { get; set; } = new();
    public Dictionary<string, List<string>> Properties { get; set; } = new();
    public bool Available => Inventory.Quantity > 0;
    public bool Discontinued { get; set; }
}