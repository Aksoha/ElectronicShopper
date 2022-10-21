using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Data;

public interface IOrderData
{
    Task AddOrder(OrderModel order);
}