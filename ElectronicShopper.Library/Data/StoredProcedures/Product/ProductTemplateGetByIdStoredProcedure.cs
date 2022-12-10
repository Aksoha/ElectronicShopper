namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that retrieves product templates by Id from the database.
/// </summary>
/// <seealso cref="ProductTemplateModel" />
internal class ProductTemplateGetByIdStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the object to retrieve.
    /// </summary>
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_GetById";
    }
}