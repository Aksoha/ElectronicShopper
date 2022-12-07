namespace ElectronicShopper.Library.StoredProcedures.Category;


[Obsolete("Filter data from ICategoryData.GetAll instead of calling database")]
public class CategoryGetAncestorsStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }
    public string ProcedureName()
    {
        return "spCategory_GetAncestors";
    }
}