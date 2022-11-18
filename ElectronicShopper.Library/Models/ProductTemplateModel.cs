namespace ElectronicShopper.Library.Models;

public class ProductTemplateModel : IDbEntity
{
    public int? Id { get; set; }
    public List<string> Properties { get; set; } = new();
}