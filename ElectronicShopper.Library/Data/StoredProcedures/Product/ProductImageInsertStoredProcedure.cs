namespace ElectronicShopper.Library.StoredProcedures.Product;

internal class ProductImageInsertStoredProcedure : IStoredProcedure
{
    public int ProductId { get; set; }
    public string Path { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public string ProcedureName()
    {
        return "spProductImage_Insert";
    }
}