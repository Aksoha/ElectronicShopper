namespace ElectronicShopper.Library.StoredProcedures.Product;

public class ProductTemplateGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spProductTemplate_GetAll";
    }
}