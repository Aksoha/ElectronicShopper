using ElectronicShopper.Library;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ElectronicShopper.Pages;

/// <summary>
///     Cart page.
/// </summary>
[Authorize]
public partial class Cart : ComponentBase, IDisposable
{
    /// <summary>
    ///     List containing all the product images that will be displayed on the page.
    /// </summary>
    private readonly List<(int ProductId, string Path)> _images = new();

    private EditContext _editContext = null!;

    /// <summary>
    ///     List of errors that will be displayed on invalid input.
    /// </summary>
    private string _errorMessage = "";

    [Inject] private ICartService CartService { get; set; } = default!;
    [Inject] private IProductData ProductData { get; set; } = default!;
    [Inject] private Navigation NavManager { get; set; } = default!;

    public void Dispose()
    {
        CartService.CartCountChange -= UpdatePage;
    }

    protected override async Task OnInitializedAsync()
    {
        CartService.CartCountChange += UpdatePage;
        _editContext = new EditContext(CartService);
        await FetchImages();
    }

    private void RemoveItemFromCart(OrderDetailModel item)
    {
        CartService.Remove(item);
    }

    private void Checkout()
    {
        _errorMessage = "";
        CartService.Checkout();
        NavManager.NavigateTo("/order-confirmation");
    }

    /// <summary>
    ///     Redirect to page containing specific product.
    /// </summary>
    /// <param name="item">Product to redirect to.</param>
    private void GoToProduct(OrderDetailModel item)
    {
        NavManager.NavigateTo($"/p/{item.ProductId}");
    }

    /// <summary>
    ///     Retrieve image of the product
    /// </summary>
    /// <param name="item">The product.</param>
    private string GetProductImage(OrderDetailModel item)
    {
        var output = _images.SingleOrDefault(x => x.ProductId == item.ProductId).Path;
        return output;
    }


    /// <summary>
    ///     Retrieves product images for all the products in the cart.
    /// </summary>
    private async Task FetchImages()
    {
        _images.Clear();
        foreach (var item in CartService)
        {
            var images = await ProductData.GetProductImages(new ProductModel { Id = item.ProductId });
            var primaryImg = images.SingleOrDefault(x => x.IsPrimary)?.Path;

            _images.Add(new ValueTuple<int, string>((int)item.ProductId!, primaryImg ?? "./images/missing-image.png"));
        }
    }

    /// <summary>
    ///     Re-renders component whenever cart item changes.
    /// </summary>
    /// <remarks>
    ///     Calling this method is required due to problems with fetching data from CartService.
    ///     Refreshed page did not display cart items despite them being in the cart.
    /// </remarks>
    private async void UpdatePage()
    {
        await FetchImages();
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    ///     Adds an error for invalid submit.
    /// </summary>
    private void InvalidSubmit()
    {
        _errorMessage = "Item count can't contain non positive value";
    }
}