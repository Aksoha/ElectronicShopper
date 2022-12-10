using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Product;

/// <summary>
///     Represents a stored procedure that inserts into ProductTemplate table.
/// </summary>
internal class ProductTemplateInsertStoredProcedure : IStoredProcedure
{
    /// <inheritdoc cref="ProductTemplateDbModel.Name" />
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc cref="ProductTemplateDbModel.Properties" />
    public string? Properties { get; set; }

    public string ProcedureName()
    {
        return "spProductTemplate_Insert";
    }
}