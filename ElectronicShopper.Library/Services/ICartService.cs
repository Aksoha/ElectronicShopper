namespace ElectronicShopper.Library.Services;

/// <summary>
///     Provides access to the shopping cart.
/// </summary>
public interface ICartService : ICollection<OrderDetailModel>
{
    /// <summary>
    ///     Price of all items in the cart.
    /// </summary>
    /// <seealso cref="InventoryModel.Price" />
    decimal TotalPrice { get; }

    /// <summary>
    ///     The sum of all products in the cart multiplied by the count of the product.
    /// </summary>
    new int Count { get; }

    event Action CartCountChange;

    /// <summary>
    ///     Clears the cart.
    /// </summary>
    new void Clear();

    /// <summary>
    ///     Finalizes purchase.
    /// </summary>
    Task Checkout();
}