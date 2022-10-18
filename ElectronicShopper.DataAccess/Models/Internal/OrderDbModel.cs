namespace ElectronicShopper.DataAccess.Models.Internal;

internal class OrderDbModel : IDbEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime PurchaseTime { get; set; }
}