namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     This class represents database Inventory table.
/// </summary>
/// <seealso cref="InventoryModel" />
internal class InventoryDbModel : IDbEntity
{
    /// <summary>
    ///     Id of the product associated with inventory.
    /// </summary>
    public int ProductId { get; set; }
    
    /// <inheritdoc cref="InventoryModel.Quantity" />
    public int QuantityAvailable { get; set; }

    /// <inheritdoc cref="InventoryModel.Reserved" />
    public int QuantityReserved { get; set; }

    /// <inheritdoc cref="InventoryModel.Price" />
    public decimal Price { get; set; }

    public int Id { get; set; }
}