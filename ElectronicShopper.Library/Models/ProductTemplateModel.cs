namespace ElectronicShopper.Library.Models;

public class ProductTemplateModel : IDbEntity
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Properties { get; set; } = new();
}