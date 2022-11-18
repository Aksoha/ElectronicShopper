using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Data;

/// <summary>
///     Provides access to order table stored in the database.
/// </summary>
public interface IOrderData
{
    /// <summary>
    ///     Adds new order to the database.
    /// </summary>
    /// <param name="order">Order to add.</param>
    /// <exception cref="FluentValidation.ValidationException">
    ///     Thrown when <paramref name="order" />
    ///     does not pass data validation.
    /// </exception>
    Task Create(OrderModel order);
}