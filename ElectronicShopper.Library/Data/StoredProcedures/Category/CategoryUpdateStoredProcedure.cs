namespace ElectronicShopper.Library.StoredProcedures.Category;

internal class CategoryUpdateStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public string ProcedureName()
    {
        return "spCategory_Update";
    }
}