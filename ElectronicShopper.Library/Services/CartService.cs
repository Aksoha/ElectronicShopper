using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Identity;
using Microsoft.AspNetCore.Http;

namespace ElectronicShopper.Library.Services;

public class CartService : ICartService
{
    public event Action? CartCountChange;

    [ValidateComplexType] private List<OrderDetailModel> Products { get; set; } = new();
    private readonly IOrderData _orderData;
    private readonly IHttpContextAccessor _accessor;
    private readonly ApplicationUserManager _userManager;
    private readonly ILocalStorageService _localStorage;
    private const string CartCacheName = "CartItems";
    private const string CartCacheTime = "CartTime";

    public CartService(IOrderData orderData, IHttpContextAccessor accessor, ApplicationUserManager userManager,
        ILocalStorageService localStorage)
    {
        ArgumentNullException.ThrowIfNull(localStorage);
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(orderData);

        _orderData = orderData;
        _accessor = accessor;
        _userManager = userManager;
        _localStorage = localStorage;
        Task.Run(LoadCache);
    }

    public bool IsReadOnly => false;
    public int Count => Products.Sum(x => x.Quantity);
    public decimal TotalPrice => Products.Sum(x => x.PricePerItem * x.Quantity);


    public void Add(OrderDetailModel item)
    {
        var existingItem = Products.SingleOrDefault(x => x.ProductId == item.ProductId);
        if (existingItem is not null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            Products.Add(item);
            item.PropertyChanged += ProductChanged;
        }

        Task.Run(SetCache);
        CartCountChange?.Invoke();
    }

    public bool Remove(OrderDetailModel item)
    {
        var result = Products.Remove(item);
        item.PropertyChanged -= ProductChanged;
        Task.Run(SetCache);
        CartCountChange?.Invoke();
        return result;
    }

    public async Task Checkout()
    {
        ArgumentNullException.ThrowIfNull(_accessor.HttpContext);

        var loggedUser = await _userManager.GetUserAsync(_accessor.HttpContext.User);
        if (loggedUser is null)
            throw new ArgumentNullException(nameof(loggedUser));

        var order = new OrderModel
        {
            UserId = loggedUser.Id,
            PurchasedProducts = Products
        };

        await _orderData.Create(order);


        Clear();
    }

    /// <summary>
    /// Clears cart
    /// </summary>
    public void Clear()
    {
        UnsubscribeFromProductEvent();
        Products.Clear();
        Task.Run(SetCache);
        CartCountChange?.Invoke();
    }

    public bool Contains(OrderDetailModel item)
    {
        return Products.Contains(item);
    }

    public void CopyTo(OrderDetailModel[] array, int arrayIndex)
    {
        Products.CopyTo(array, arrayIndex);
    }


    public IEnumerator<OrderDetailModel> GetEnumerator()
    {
        return Products.GetEnumerator();
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
        foreach (var product in Products)
        {
            product.PropertyChanged -= ProductChanged;
        }
    }

    private async Task LoadCache()
    {
        var cacheDate = await _localStorage.GetItemAsync<DateTime?>(CartCacheTime);
        if (cacheDate is not null && DateTime.UtcNow < ((DateTime)cacheDate).AddDays(7))
        {
            Products = await _localStorage.GetItemAsync<List<OrderDetailModel>>(CartCacheName);
        }

        foreach (var product in Products)
        {
            product.PropertyChanged += ProductChanged;
        }

        CartCountChange?.Invoke();
    }

    private async Task SetCache()
    {
        await _localStorage.SetItemAsync(CartCacheName, Products);
        await _localStorage.SetItemAsync(CartCacheTime, DateTime.UtcNow);
    }

    public void Dispose()
    {
        UnsubscribeFromProductEvent();
    }
}