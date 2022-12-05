namespace ElectronicShopper.Library.Models.Internal;

internal class OrderDetailDbModel : IDbEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
}