namespace ElectronicShopper.Library.StoredProcedures.Category;


[Obsolete("Filter data from ICategoryData.GetAll instead of calling database")]
internal class CategoryGetAllLeafsStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetAllLeafs";
    }
}