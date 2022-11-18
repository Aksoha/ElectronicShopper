namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductTemplateInsertStoredProcedure : IStoredProcedure
{
    public string? Properties { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_Insert";
    }
}