using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.Models;

/// <summary>
///     A class used for creating new <see cref="CategoryModel" /> in the database.
/// </summary>
public class CategoryCreateModel : IDbEntity
{
    /// <inheritdoc cref="CategoryDbModel.CategoryName" />
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="CategoryDbModel.ParentId" />
    public int? ParentId { get; set; }

    public int? Id { get; set; }
}