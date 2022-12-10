using ElectronicShopper.Library;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Shared;

/// <summary>
///     A component displayed below <see cref="AppBar" /> that holds information's that displays all root level categories.
/// </summary>
public partial class CategoryBar : ComponentBase
{
    private IEnumerable<CategoryModel>? _categories;
    [Inject] private ICategoryData CategoryData { get; set; } = default!;
    [Inject] private Navigation NavManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _categories = await CategoryData.GetRootCategories();
    }

    /// <summary>
    ///     Redirects to a specific category page.
    /// </summary>
    /// <param name="selectedCategory">The selected category.</param>
    private void OnCategorySelected(CategoryModel selectedCategory)
    {
        NavManager.NavigateTo($"/c/{selectedCategory.Id}");
    }
}