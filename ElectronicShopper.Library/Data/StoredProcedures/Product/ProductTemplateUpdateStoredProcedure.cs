using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that updates ProductTemplate table.
/// </summary>
internal class ProductTemplateUpdateStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of product that is to be updated.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Id { get; set; }

    /// <summary>
    ///     New name of the template.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     New <see cref="ProductTemplateDbModel.Properties" />.
    /// </summary>
    public string? Properties { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_Update";
    }
}