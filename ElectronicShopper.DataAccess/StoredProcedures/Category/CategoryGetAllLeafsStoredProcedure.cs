namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

internal class CategoryGetAllLeafsStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetAllLeafs";
    }
}