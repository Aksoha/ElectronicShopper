namespace ElectronicShopper.Library.Models.Internal;

internal class InventoryDbModel : IDbEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityReserved { get; set; }
    public decimal Price { get; set; }
}