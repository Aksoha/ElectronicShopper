using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that retrieves all parent categories for given category from the database.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
[Obsolete("Filter data from ICategoryData.GetAll instead of calling database")]
public class CategoryGetAncestorsStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of category whose parents are to be retrieved.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spCategory_GetAncestors";
    }
}