namespace ElectronicShopper.Library.StoredProcedures.Order;

internal class OrderDetailsInsertStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerItem { get; set; }

    public string ProcedureName()
    {
        return "spOrderDetails_Insert";
    }
}