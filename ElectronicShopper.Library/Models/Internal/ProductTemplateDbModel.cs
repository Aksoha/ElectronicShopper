namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     This class represents database ProductImage table.
/// </summary>
/// <seealso cref="ProductTemplateModel" />
internal class ProductTemplateDbModel : IDbEntity
{
    /// <inheritdoc cref="ProductTemplateModel.Name" />
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     A list of names of properties stored as JSON.
    /// </summary>
    /// <seealso cref="ProductTemplateModel.Properties" />
    public string? Properties { get; set; }

    /// <summary>
    ///     UTC time when template was created.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    ///     UTC time when template was last modified.
    /// </summary>
    public DateTime? ModificationDate { get; set; }

    public int Id { get; set; }
}