namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that deletes ProductTemplate row.
/// </summary>
/// <seealso cref="ProductTemplateModel" />
internal class ProductTemplateDeleteStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the object to delete.
    /// </summary>
    public int Id { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_Delete";
    }
}