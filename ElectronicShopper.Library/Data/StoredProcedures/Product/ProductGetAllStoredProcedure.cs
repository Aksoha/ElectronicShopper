namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that retrieves all products from the database.
/// </summary>
/// <see cref="ProductModel"/>
internal class ProductGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spProduct_GetAll";
    }
}