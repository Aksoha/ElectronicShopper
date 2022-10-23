using System.Collections;
using System.ComponentModel;
using Blazored.LocalStorage;
using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.DataAccess.Identity;
using ElectronicShopper.Library.Models;


namespace ElectronicShopper.Services;

public class CartService : ICartService
{
    public event Action? CartCountChange;
    private List<OrderDetailModel> _products = new();
    private readonly IOrderData _orderData;
    private readonly IHttpContextAccessor _accessor;
    private readonly ApplicationUserManager _userManager;
    private readonly ILocalStorageService _localStorage;
    private const string CartCacheName = "CartItems";
    private const string CartCacheTime = "CartTime";

    public CartService(IOrderData orderData, IHttpContextAccessor accessor, ApplicationUserManager userManager,
        ILocalStorageService localStorage)
    {
        _orderData = orderData;
        _accessor = accessor;
        _userManager = userManager;
        _localStorage = localStorage;
        Task.Run(LoadCache);
    }

    public bool IsReadOnly => false;
    public int Count => _products.Sum(x => x.Quantity);
    public decimal TotalPrice => _products.Sum(x => x.PricePerItem * x.Quantity);


    public void Add(OrderDetailModel item)
    {
        var existingItem = _products.SingleOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem is not null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            _products.Add(item);
            item.PropertyChanged += ProductChanged;
        }

        Task.Run(SetCache);
        CartCountChange?.Invoke();
    }

    public bool Remove(OrderDetailModel item)
    {
        var result = _products.Remove(item);
        item.PropertyChanged -= ProductChanged;
        Task.Run(SetCache);
        CartCountChange?.Invoke();
        return result;
    }

    public async Task Checkout()
    {
        var loggedUser = await _userManager.GetUserAsync(_accessor.HttpContext?.User);
        if (loggedUser is null)
            throw new ArgumentNullException(nameof(loggedUser));

        var order = new OrderModel
        {
            UserId = loggedUser.Id,
            PurchasedProducts = _products
        };

        await _orderData.AddOrder(order);
        Clear();
    }

    /// <summary>
    /// Clears cart
    /// </summary>
    public void Clear()
    {
        UnsubscribeFromProductEvent();
        _products.Clear();
        Task.Run(SetCache);
        CartCountChange?.Invoke();
    }

    public bool Contains(OrderDetailModel item)
    {
        return _products.Contains(item);
    }

    public void CopyTo(OrderDetailModel[] array, int arrayIndex)
    {
        _products.CopyTo(array, arrayIndex);
    }


    public IEnumerator<OrderDetailModel> GetEnumerator()
    {
        return _products.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void ProductChanged(object? sender, PropertyChangedEventArgs e)
    {
        Task.Run(SetCache);
        CartCountChange?.Invoke();
    }

    private void UnsubscribeFromProductEvent()
    {
        foreach (var product in _products)
        {
            product.PropertyChanged -= ProductChanged;
        }
    }

    private async Task LoadCache()
    {
        var cacheDate = await _localStorage.GetItemAsync<DateTime?>(CartCacheTime);
        if (cacheDate is not null && DateTime.UtcNow < ((DateTime)cacheDate).AddDays(7))
        {
            _products = await _localStorage.GetItemAsync<List<OrderDetailModel>>(CartCacheName);
        }

        foreach (var product in _products)
        {
            product.PropertyChanged += ProductChanged;
        }

        CartCountChange?.Invoke();
    }

    private async Task SetCache()
    {
        await _localStorage.SetItemAsync(CartCacheName, _products);
        await _localStorage.SetItemAsync(CartCacheTime, DateTime.UtcNow);
    }

    public void Dispose()
    {
        UnsubscribeFromProductEvent();
    }
}