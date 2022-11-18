namespace ElectronicShopper.DataAccess.StoredProcedures.Product;

internal class ProductGetStoredProcedure : IStoredProcedure
{
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProduct_Get";
    }
}