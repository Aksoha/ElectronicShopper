namespace ElectronicShopper.Library.Models;

public class ProductTemplateModel
{
    public int Id { get; set; }
    public List<string> Properties { get; set; } = new();
}