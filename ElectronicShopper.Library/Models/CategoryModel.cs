namespace ElectronicShopper.Library.Models;

public class CategoryModel : IDbEntity
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryModel? Parent { get; set; }
}