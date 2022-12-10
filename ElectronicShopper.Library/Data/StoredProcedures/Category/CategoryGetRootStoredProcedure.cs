using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that retrieves categories from the database.
///     Retrieves only categories that do not have parent category.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
[Obsolete("Filter data from ICategoryData.GetAll instead of calling database")]
internal class CategoryGetRootStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetRoot";
    }
}