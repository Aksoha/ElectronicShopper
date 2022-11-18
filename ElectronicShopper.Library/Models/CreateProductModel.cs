using System.ComponentModel.DataAnnotations;

namespace ElectronicShopper.Library.Models;

public class CreateProductModel
{
    [Required] public string ProductName { get; set; } = string.Empty;

    [Required] public decimal? RetailPrice { get; set; }

    public CategoryModel Category { get; set; } = new();
    public List<ProductImageModel> Images { get; set; } = new();
    public Dictionary<string, List<string>> Properties { get; set; } = new();
}