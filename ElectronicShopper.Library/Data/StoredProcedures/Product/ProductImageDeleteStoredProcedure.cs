namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that deletes product image from the database.
/// </summary>
internal class ProductImageDeleteStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of image to delete.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProductImage_Delete";
    }
}