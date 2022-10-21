namespace ElectronicShopper.Library.Models;

public class OrderDetailModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
}