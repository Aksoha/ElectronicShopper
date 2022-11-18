namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

public class CategoryGetAncestorsStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }
    public string ProcedureName()
    {
        return "spCategory_GetAncestors";
    }
}