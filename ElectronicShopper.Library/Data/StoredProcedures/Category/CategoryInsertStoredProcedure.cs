using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that inserts into Category table.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
internal class CategoryInsertStoredProcedure : IStoredProcedure
{

    /// <inheritdoc cref="CategoryDbModel.ParentId" />
    public int? ParentId { get; set; }


    /// <inheritdoc cref="CategoryDbModel.CategoryName" />
    public string CategoryName { get; set; } = string.Empty;

    public string ProcedureName()
    {
        return "spCategory_Insert";
    }
}