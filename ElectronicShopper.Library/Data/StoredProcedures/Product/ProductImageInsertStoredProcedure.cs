using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that inserts into ProductImage table.
/// </summary>
/// <seealso cref="ProductImageModel" />
/// <seealso cref="ProductImageDbModel" />
internal class ProductImageInsertStoredProcedure : IStoredProcedure
{
    /// <inheritdoc cref="ProductImageDbModel.ProductId" />
    public int ProductId { get; set; }


    /// <inheritdoc cref="ProductImageDbModel.Path" />
    public string Path { get; set; } = string.Empty;

    /// <inheritdoc cref="ProductImageDbModel.IsPrimary" />
    public bool IsPrimary { get; set; }

    public string ProcedureName()
    {
        return "spProductImage_Insert";
    }
}