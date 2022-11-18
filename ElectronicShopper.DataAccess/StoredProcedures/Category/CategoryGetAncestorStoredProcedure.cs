namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

internal class CategoryGetAncestorStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spCategory_GetAncestor";
    }
}