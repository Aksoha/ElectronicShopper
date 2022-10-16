namespace ElectronicShopper.Library.Models;

public class ProductModel
{
    public int Id { get; set; }
    public CategoryModel Category { get; set; } = new();
    public decimal RetailPrice { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public Dictionary<string, List<string>> Properties { get; set; } = new();
}