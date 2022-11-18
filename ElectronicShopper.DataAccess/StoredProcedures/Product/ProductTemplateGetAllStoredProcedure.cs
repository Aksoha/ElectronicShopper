namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

public class ProductTemplateGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spProductTemplate_GetAll";
    }
}