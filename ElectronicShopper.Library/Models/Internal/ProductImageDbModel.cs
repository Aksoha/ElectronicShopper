namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     This class represents database ProductImage table.
/// </summary>
/// <seealso cref="ProductImageModel" />
internal class ProductImageDbModel : IDbEntity
{
    /// <summary>
    ///     Id of the product associated with the image.
    /// </summary>
    public int ProductId { get; set; }


    /// <inheritdoc cref="ProductImageModel.Path" />
    public string Path { get; set; } = string.Empty;

    /// <inheritdoc cref="ProductImageModel.IsPrimary" />
    public bool IsPrimary { get; set; }

    public int Id { get; set; }
}