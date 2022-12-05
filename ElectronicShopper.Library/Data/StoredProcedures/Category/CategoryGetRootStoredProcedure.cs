namespace ElectronicShopper.Library.StoredProcedures.Category;

internal class CategoryGetRootStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetRoot";
    }
}