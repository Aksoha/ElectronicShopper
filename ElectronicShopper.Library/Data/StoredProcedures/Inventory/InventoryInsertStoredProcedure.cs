using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Inventory;

/// <summary>
///     Represents a stored procedure that inserts into Inventory table.
/// </summary>
/// <seealso cref="InventoryModel" />
/// <seealso cref="InventoryDbModel" />
internal class InventoryInsertStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the product whose inventory is to be inserted.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int ProductId { get; set; }


    /// <inheritdoc cref="InventoryDbModel.QuantityAvailable" />
    public int QuantityAvailable { get; set; }


    /// <inheritdoc cref="InventoryDbModel.QuantityReserved" />
    public int QuantityReserved { get; set; }

    /// <inheritdoc cref="InventoryDbModel.Price" />
    public decimal Price { get; set; }

    public string ProcedureName()
    {
        return "spInventory_Insert";
    }
}