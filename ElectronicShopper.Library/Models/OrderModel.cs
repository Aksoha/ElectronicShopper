namespace ElectronicShopper.Library.Models;

public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderDetailModel> PurchasedProducts { get; set; } = new();
    public DateTime PurchaseTime { get; set; }
}