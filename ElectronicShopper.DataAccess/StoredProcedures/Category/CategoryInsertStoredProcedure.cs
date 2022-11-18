namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

internal class CategoryInsertStoredProcedure : IStoredProcedure
{
    public int? ParentId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public string ProcedureName()
    {
        return "spCategory_Insert";
    }
}