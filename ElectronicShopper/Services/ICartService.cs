using ElectronicShopper.Library.Models;

namespace ElectronicShopper.Services;

public interface ICartService : ICollection<OrderDetailModel>
{
    decimal TotalPrice { get; }
    event Action CartCountChange;
    Task Checkout();
}