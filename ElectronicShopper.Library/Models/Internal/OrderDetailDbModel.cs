namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     This class represents database OrderDetail table in the database.
/// </summary>
/// <seealso cref="OrderDetailModel" />
/// <seealso cref="OrderDbModel" />
internal class OrderDetailDbModel : IDbEntity
{
    /// <summary>
    ///     Id of <see cref="OrderDbModel"/>.
    /// </summary>
    public int OrderId { get; set; }

    /// <inheritdoc cref="OrderDetailModel.ProductId"/>
    public int ProductId { get; set; }

    /// <inheritdoc cref="OrderDetailModel.Quantity"/>
    public int Quantity { get; set; }
    
    /// <inheritdoc cref="OrderDetailModel.PricePerItem"/>
    public decimal PricePerItem { get; set; }

    public int Id { get; set; }
}