namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     This class represents database Order table in the database.
/// </summary>
/// <seealso cref="OrderModel" />
internal class OrderDbModel : IDbEntity
{
    /// <inheritdoc cref="OrderModel.UserId" />
    public int UserId { get; set; }

    /// <inheritdoc cref="OrderModel.PurchaseTime" />
    public DateTime PurchaseTime { get; set; }

    public int Id { get; set; }
}