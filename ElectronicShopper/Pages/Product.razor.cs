using ElectronicShopper.Library;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Services;
using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Pages;

/// <summary>
///     Product page. Contains detailed information about product.
/// </summary>
public partial class Product : ComponentBase
{
    private readonly OrderDetailModel _order = new();
    private List<string> _images = new();
    private string _previewImage = "";

    private ProductModel? _product;
    private string _selectedImage = "";
    [Inject] private ICartService CartService { get; set; } = default!;
    [Inject] private IProductData ProductData { get; set; } = default!;
    [Inject] private Navigation NavManager { get; set; } = default!;


    /// <summary>
    ///     Id of the product.
    /// </summary>
    [Parameter]
    public int Id { get; set; }


    protected override async Task OnInitializedAsync()
    {
        var p = await ProductData.Get(Id);
        if (p is not null)
        {
            _product = p;
            var productImages = await ProductData.GetProductImages(_product);

            var primaryImg = productImages.SingleOrDefault(x => x.IsPrimary);
            _selectedImage = primaryImg is not null ? primaryImg.Path : "./images/missing-image.png";

            _images = productImages.Select(image => image.Path).ToList();
            _order.Quantity = 1;
            _order.ProductId = _product.Id;
            _order.ProductName = _product.ProductName;
            _order.PricePerItem = _product.Inventory.Price;

            _previewImage = _selectedImage;
        }
        else
        {
            // handle navigation to non existing product
            NavManager.NavigateTo("/");
        }
    }

    /// <summary>
    ///     Adds product to the cart.
    /// </summary>
    private void AddToCart()
    {
        CartService.Add(_order);
    }


    /// <summary>
    ///     Changes the big image to one that is currently hovered over.
    /// </summary>
    /// <param name="image">The image.</param>
    private void ImageMouseOver(string image)
    {
        _previewImage = image;
        StateHasChanged();
    }


    /// <summary>
    ///     Resets the big image to the one that was selected in case of mouse moving out.
    /// </summary>
    private void ImageMouseOut()
    {
        _previewImage = _selectedImage;
        StateHasChanged();
    }

    /// <summary>
    ///     Changes the big image in the image preview section.
    /// </summary>
    /// <param name="image">The image.</param>
    private void ImageMouseClick(string image)
    {
        _selectedImage = image;
        _previewImage = image;
        StateHasChanged();
    }
}