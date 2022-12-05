namespace ElectronicShopper.Library.StoredProcedures.Category;

internal class CategoryGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetAll";
    }
}