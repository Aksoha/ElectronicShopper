namespace ElectronicShopper.DataAccess.Models.Internal;

internal class CategoryDbModel : IDbEntity
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}