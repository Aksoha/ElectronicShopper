namespace ElectronicShopper.Library.Models;

public class InventoryModel
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int Reserved { get; set; }
}