namespace ElectronicShopper.DataAccess.StoredProcedures.Inventory;

internal class InventoryUpdateStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }
    public int QuantityAvailable { get; set; }

    public int QuantityReserved { get; set; }

    public decimal Price { get; set; }
    public string ProcedureName()
    {
        return "spInventory_Update";
    }
}