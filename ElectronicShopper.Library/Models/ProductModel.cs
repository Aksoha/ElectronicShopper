namespace ElectronicShopper.Library.Models;

public class ProductModel
{
    public int Id { get; set; }
    public CategoryModel Category { get; set; } = new();
    public decimal RetailPrice { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<ProductImageModel> Images { get; set; } = new();
    public Dictionary<string, List<string>> Properties { get; set; } = new();
}