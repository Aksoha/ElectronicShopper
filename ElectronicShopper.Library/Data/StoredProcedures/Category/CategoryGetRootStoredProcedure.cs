namespace ElectronicShopper.Library.StoredProcedures.Category;


[Obsolete("Filter data from ICategoryData.GetAll instead of calling database")]
internal class CategoryGetRootStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetRoot";
    }
}