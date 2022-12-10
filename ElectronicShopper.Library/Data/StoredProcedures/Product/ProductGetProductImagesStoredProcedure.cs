namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that retrieves all images associated with product.
/// </summary>
internal class ProductGetProductImagesStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of product whose images are to be retrieved.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int ProductId { get; set; }

    public string ProcedureName()
    {
        return "spProduct_GetProductImages";
    }
}