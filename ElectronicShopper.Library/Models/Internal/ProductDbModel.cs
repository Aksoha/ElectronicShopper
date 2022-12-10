namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     Represents a stored procedure that inserts into Product table.
/// </summary>
/// <seealso cref="ProductModel" />
internal class ProductDbModel : IDbEntity
{
    /// <summary>
    ///     Id of the category that product belongs to.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    ///     Id of the templated used to create product.
    /// </summary>
    public int? ProductTemplateId { get; set; }

    /// <inheritdoc cref="ProductModel.ProductName" />
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    ///     A list of properties stored as JSON.
    /// </summary>
    /// <seealso cref="ProductModel.Properties" />
    public string? Properties { get; set; }

    /// <inheritdoc cref="ProductModel.Discontinued" />
    public bool Discontinued { get; set; }

    public int Id { get; set; }
}