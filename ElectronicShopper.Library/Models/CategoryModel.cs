namespace ElectronicShopper.Library.Models;

/// <summary>
///     A category of product. Categories are represented in the application as a tree structure.
/// </summary>
public class CategoryModel : IDbEntity
{
    /// <summary>
    ///     Name of category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Parent category.
    /// </summary>
    public CategoryModel? Parent { get; set; }

    public int? Id { get; set; }
}