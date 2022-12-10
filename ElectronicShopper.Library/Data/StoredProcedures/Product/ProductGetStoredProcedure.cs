namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that retrieves product by Id from the database.
/// </summary>
internal class ProductGetStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of product to retrieve from the database.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProduct_Get";
    }
}