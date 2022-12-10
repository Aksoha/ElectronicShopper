using ElectronicShopper.Library.Models.Internal;

namespace ElectronicShopper.Library.StoredProcedures.Order;

/// <summary>
///     Represents a stored procedure that inserts into OrderDetails table.
/// </summary>
/// <seealso cref="OrderDetailModel" />
/// <seealso cref="OrderDetailDbModel" />
/// <seealso cref="OrderInsertStoredProcedure" />
internal class OrderDetailsInsertStoredProcedure : IStoredProcedure
{
    /// <summary>
    ///     Id of the purchased product.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int ProductId { get; set; }

    /// <summary>
    ///     Id of the purchased product.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int OrderId { get; set; }

    /// <summary>
    ///     Id of the purchased product.
    /// </summary>
    /// <seealso cref="Models.Internal.IDbEntity" />
    public int Quantity { get; set; }

    /// <summary>
    ///     Id of the purchased product.
    /// </summary>
    /// <seealso cref="OrderDetailDbModel.PricePerItem" />
    public decimal PricePerItem { get; set; }

    public string ProcedureName()
    {
        return "spOrderDetails_Insert";
    }
}