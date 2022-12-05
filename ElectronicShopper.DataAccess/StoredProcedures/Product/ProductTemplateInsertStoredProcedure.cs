namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductTemplateInsertStoredProcedure : IStoredProcedure
{
    public string Name { get; set; } = string.Empty;
    public string? Properties { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_Insert";
    }
}