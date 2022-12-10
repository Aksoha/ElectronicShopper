using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that updates Category table.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
internal class CategoryUpdateStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of category that is to be updated.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    /// <summary>
    ///     New parent Id.
    /// </summary>
    /// <seealso cref="CategoryDbModel.ParentId" />
    /// <seealso cref="CategoryModel.Parent" />
    public int ParentId { get; set; }

    /// <summary>
    ///     New category name.
    /// </summary>
    /// <seealso cref="CategoryDbModel.CategoryName" />
    public string CategoryName { get; set; } = string.Empty;

    public string ProcedureName()
    {
        return "spCategory_Update";
    }
}