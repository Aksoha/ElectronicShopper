namespace ElectronicShopper.Library.Models;

/// <summary>
///     Represents a order containing information about all the products that were purchased by the user in single
///     transaction.
/// </summary>
public class OrderModel : IDbEntity
{
    /// <summary>
    ///     Id of an user who made a purchase.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///     A collection of items that were purchased.
    /// </summary>
    public List<OrderDetailModel> PurchasedProducts { get; set; } = new();

    /// <summary>
    ///     Time when purchase was made.
    /// </summary>
    public DateTime PurchaseTime { get; set; }

    public int? Id { get; set; }
}