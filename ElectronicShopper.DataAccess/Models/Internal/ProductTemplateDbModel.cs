namespace ElectronicShopper.DataAccess.Models.Internal;

internal class ProductTemplateDbModel : IDbEntity
{
    public int Id { get; set; }
    public string? Properties { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
}