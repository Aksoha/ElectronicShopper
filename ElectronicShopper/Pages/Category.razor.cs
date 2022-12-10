using System.ComponentModel.DataAnnotations;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Services;
using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Pages;

/// <summary>
///     Category page. Contains list of products which are under specific category (or child of this category).
/// </summary>
public partial class Category : ComponentBase
{
    /// <summary>
    ///     Products that match the filter.
    /// </summary>
    private IEnumerable<ProductModel> _filteredProducts = null!;

    /// <summary>
    ///     Maximum <see cref="InventoryModel.Price" />. Products above threshold will not be displayed.
    /// </summary>
    private decimal _priceThreshold;

    private IEnumerable<ProductModel>? _products;

    /// <summary>
    ///     Currently selected <see cref="ProductSort">sorting</see> method.
    /// </summary>
    private ProductSort _selectedSortMethod;

    [Inject] private IProductData ProductData { get; set; } = default!;
    [Inject] private Navigation NavManager { get; set; } = default!;
    [Inject] private ICartService CartService { get; set; } = default!;


    /// <summary>
    ///     Id of the category.
    /// </summary>
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var products = await ProductData.GetAll();

        // find all products that are in this category or any child category
        var p = new List<ProductModel>();
        foreach (var product in products)
        {
            var category = product.Category;

            while (category is not null)
            {
                if (category.Id == Id)
                {
                    p.Add(product);
                    break;
                }

                category = category.Parent;
            }
        }

        _products = p;

        // set default filter
        var filteredProducts = _products.ToList();
        _filteredProducts = filteredProducts;

        if (filteredProducts.Count == 0)
            return;

        _selectedSortMethod = ProductSort.NameAscending;
        _priceThreshold = filteredProducts.Select(x => x.Inventory.Price).Max();
        FilterProductPrice();
    }

    /// <summary>
    ///     Redirects to <see cref="Product" /> page.
    /// </summary>
    /// <param name="item">Product to navigate.</param>
    private void GoToProduct(ProductModel item)
    {
        NavManager.NavigateTo($"/p/{item.Id}");
    }


    /// <summary>
    ///     Adds product to the cart.
    /// </summary>
    /// <param name="item">Product to add.</param>
    private void AddToCart(ProductModel item)
    {
        var order = new OrderDetailModel
        {
            Quantity = 1,
            ProductId = item.Id,
            PricePerItem = item.Inventory.Price,
            ProductName = item.ProductName
        };

        CartService.Add(order);
    }

    /// <summary>
    ///     Retrieve primary image for the product.
    /// </summary>
    /// <param name="item">The product.</param>
    /// <returns>A path containing image.</returns>
    private string GetProductImage(ProductModel item)
    {
        var image = item.Images.SingleOrDefault(x => x.IsPrimary);
        return image is null ? "./images/missing-image.png" : image.Path;
    }


    /// <summary>
    ///     Sorts a product based on <see cref="_selectedSortMethod" />.
    /// </summary>
    private void SortProducts()
    {
        _filteredProducts = _selectedSortMethod switch
        {
            ProductSort.NameAscending => _filteredProducts.OrderBy(x => x.ProductName),
            ProductSort.NameDescending => _filteredProducts.OrderByDescending(x => x.ProductName),
            ProductSort.PriceAscending => _filteredProducts.OrderBy(x => x.Inventory.Price).ToList(),
            ProductSort.PriceDescending => _filteredProducts.OrderByDescending(x => x.Inventory.Price),
            _ => _filteredProducts
        };
    }

    /// <summary>
    ///     Hides product that do not match the filter.
    /// </summary>
    private void FilterProductPrice()
    {
        _filteredProducts = _products!.Where(x => x.Inventory.Price <= _priceThreshold);
        SortProducts();
    }


    /// <summary>
    ///     Defines a sorting method for products.
    /// </summary>
    private enum ProductSort
    {
        [Display(Name = "Price ascending")] PriceAscending,
        [Display(Name = "Price descending")] PriceDescending,
        [Display(Name = "Name ascending")] NameAscending,
        [Display(Name = "Name descending")] NameDescending
    }
}