namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductTemplateGetByIdStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_GetById";
    }
}