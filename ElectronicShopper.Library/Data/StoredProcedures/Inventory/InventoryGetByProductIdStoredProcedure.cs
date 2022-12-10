using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Inventory;

/// <summary>
///     Represents a stored procedure that retrieves inventory by Id from the database.
/// </summary>
/// <seealso cref="InventoryModel" />
/// <seealso cref="InventoryDbModel" />
internal class InventoryGetByProductIdStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the product whose inventory is to be retrieved.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int ProductId { get; set; }

    public string ProcedureName()
    {
        return "spInventory_GetByProductId";
    }
}