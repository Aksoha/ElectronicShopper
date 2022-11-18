namespace ElectronicShopper.Library.Models;

public class CategoryCreateModel : IDbEntity
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}