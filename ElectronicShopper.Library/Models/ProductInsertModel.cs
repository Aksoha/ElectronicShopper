namespace ElectronicShopper.Library.Models;

public class ProductInsertModel : IDbEntity
{
    public int? Id { get; set; }
    public int? CategoryId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public ProductTemplateModel? Template { get; set; }
    public List<MemoryImageModel> Images { get; set; } = new();
    public InventoryModel Inventory { get; set; } = new();
    public Dictionary<string, List<string>> Properties { get; set; } = new();
}