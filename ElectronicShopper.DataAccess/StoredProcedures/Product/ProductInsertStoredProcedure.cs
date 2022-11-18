namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductInsertStoredProcedure : IStoredProcedure
{
    public int CategoryId { get; set; }
    public int? ProductTemplateId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Properties { get; set; }
    public bool Discontinued => false;
    public string ProcedureName()
    {
        return "spProduct_Insert";
    }
}