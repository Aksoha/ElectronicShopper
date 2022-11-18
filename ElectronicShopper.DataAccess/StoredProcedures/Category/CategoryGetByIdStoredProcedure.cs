namespace ElectronicShopper.DataAccess.StoredProcedures.Category;

internal class CategoryGetByIdStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spCategory_GetById";
    }
}