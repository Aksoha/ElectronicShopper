namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductTemplateUpdateStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }
    public string? Properties { get; set; }
    public string ProcedureName()
    {
        return "spProductTemplate_Update";
    }
}