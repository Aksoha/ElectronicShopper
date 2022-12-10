namespace ElectronicShopper.Library.Models;

/// <summary>
///     Contains information's about quantity and price of the product.
/// </summary>
public class InventoryModel : IDbEntity
{
    /// <summary>
    ///     Price of the product in euro.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    ///     Quantity of the product that determines if product can be purchased.
    /// </summary>
    /// <remarks>
    ///     Actual quantity of the product is the sum of <see cref="Quantity" /> and <see cref="Reserved" />
    ///     however reserved should only be used as a fail-safe in a case of simultaneous purchase.
    /// </remarks>
    public int Quantity { get; set; }

    /// <summary>
    ///     Quantity of the product that is kept as fail-safe in case of simultaneous purchase or a warranty.
    /// </summary>
    public int Reserved { get; set; }

    public int? Id { get; set; }
}