using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public interface IOrderData
{
    Task AddOrder(OrderModel order);
}