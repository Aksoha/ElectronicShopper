using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Inventory;

/// <summary>
///     Represents a stored procedure that updates Inventory table.
/// </summary>
/// <seealso cref="InventoryModel" />
/// <seealso cref="InventoryDbModel" />
internal class InventoryUpdateStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of product that is to be updated.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int ProductId { get; set; }


    /// <summary>
    ///     New quantity.
    /// </summary>
    /// <seealso cref="InventoryDbModel.QuantityReserved" />
    public int QuantityAvailable { get; set; }


    /// <summary>
    ///     New reserved quantity.
    /// </summary>
    /// <seealso cref="InventoryDbModel.QuantityReserved" />
    public int QuantityReserved { get; set; }


    /// <summary>
    ///     New price.
    /// </summary>
    /// <seealso cref="InventoryDbModel.Price" />
    public decimal Price { get; set; }

    public string ProcedureName()
    {
        return "spInventory_Update";
    }
}