namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that retrieves all product templates from the database.
/// </summary>
/// <seealso cref="ProductTemplateModel" />
public class ProductTemplateGetAllStoredProcedure : IStoredProcedure
{
    public string ProcedureName()
    {
        return "spProductTemplate_GetAll";
    }
}