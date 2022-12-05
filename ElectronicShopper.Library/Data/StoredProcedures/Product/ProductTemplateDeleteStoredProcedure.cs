namespace ElectronicShopper.Library.StoredProcedures.Product;

internal class ProductTemplateDeleteStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }
    
    public string ProcedureName()
    {
        return "spProductTemplate_Delete";
    }
}