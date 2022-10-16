namespace ElectronicShopper.Library.Models;

public class InventoryModel
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}