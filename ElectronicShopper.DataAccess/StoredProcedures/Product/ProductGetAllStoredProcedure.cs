namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spProduct_GetAll";
    }
}