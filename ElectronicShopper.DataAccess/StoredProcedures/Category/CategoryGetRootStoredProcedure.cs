namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

internal class CategoryGetRootStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetRoot";
    }
}