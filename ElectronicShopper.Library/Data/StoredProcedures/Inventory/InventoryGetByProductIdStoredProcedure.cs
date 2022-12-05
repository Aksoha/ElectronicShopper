namespace ElectronicShopper.Library.StoredProcedures.Inventory;

internal class InventoryGetByProductIdStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }

    public string ProcedureName()
    {
        return "spInventory_GetByProductId";
    }
}