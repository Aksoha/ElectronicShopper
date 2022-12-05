namespace ElectronicShopper.Library.StoredProcedures.Inventory;

internal class InventoryInsertStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public int QuantityReserved { get; set; }
    public decimal Price { get; set; }

    public string ProcedureName()
    {
        return "spInventory_Insert";
    }
}