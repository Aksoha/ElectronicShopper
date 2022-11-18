namespace ElectronicShopper.DataAccess.Models.Internal;

internal class ProductDbModel : IDbEntity
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int? ProductTemplateId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Properties { get; set; }
    public bool Discontinued { get; set; }
}