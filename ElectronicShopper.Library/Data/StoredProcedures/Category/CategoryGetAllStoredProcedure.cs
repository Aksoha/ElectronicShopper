using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that retrieves all categories from the database.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
internal class CategoryGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spCategory_GetAll";
    }
}