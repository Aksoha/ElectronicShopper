namespace ElectronicShopper.DataAccess.Models.Internal;

internal class ProductImageDbModel : IDbEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}