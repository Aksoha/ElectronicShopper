namespace ElectronicShopper.Library.Models;

public class OrderDetailModel
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }
}