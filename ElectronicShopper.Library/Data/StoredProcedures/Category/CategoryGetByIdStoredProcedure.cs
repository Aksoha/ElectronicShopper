using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Category;

/// <summary>
///     Represents a stored procedure that retrieves category by Id from the database.
/// </summary>
/// <seealso cref="CategoryModel" />
/// <seealso cref="CategoryDbModel" />
internal class CategoryGetByIdStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of category to retrieve from the database.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spCategory_GetById";
    }
}