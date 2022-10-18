namespace ElectronicShopper.Library.Models;

public class ProductImageModel
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}