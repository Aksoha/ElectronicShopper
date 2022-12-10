namespace ElectronicShopper.Library.Models;

/// <summary>
///     A template that helps streamlining the process of creating <see cref="ProductModel.Properties" />.
/// </summary>
/// <remarks>
///     Template can be used to create "product B" with all property names of "product A"
///     without manually specifying all names of properties.
///     It can be also used to derive properties for similar products.
/// </remarks>
public class ProductTemplateModel : IDbEntity
{
    /// <summary>
    ///     Name of template.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Names of properties associated with the product.
    /// </summary>
    /// <seealso cref="ProductModel.Properties" />
    public List<string> Properties { get; set; } = new();

    public int? Id { get; set; }
}