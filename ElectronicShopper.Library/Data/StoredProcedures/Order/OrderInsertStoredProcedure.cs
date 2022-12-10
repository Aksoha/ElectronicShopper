using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Order;

/// <summary>
///     Represents a stored procedure that inserts into Order table.
/// </summary>
/// <seealso cref="OrderModel" />
/// <seealso cref="OrderDbModel" />
/// <seealso cref="OrderDetailsInsertStoredProcedure" />
internal class OrderInsertStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the user who made a purchase.
    /// </summary>
    public int UserId { get; set; }

    public string ProcedureName()
    {
        return "spOrder_Insert";
    }
}