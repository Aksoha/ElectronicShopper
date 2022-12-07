using System.Diagnostics;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.Pages;

/// <summary>
///     Component responsible for inserting <see cref="ProductModel" /> into database.
/// </summary>
public partial class CreateProduct : ComponentBase
{
    /// <summary>
    ///     List of errors that occured during <see cref="Validate">validation</see> process.
    /// </summary>
    private readonly List<string> _errors = new();


    /// <summary>
    ///     List of all categories present in the database.
    /// </summary>
    private IEnumerable<CategoryModel> _categories = default!;


    /// <summary>
    ///     An error message that indicates that number of inserted files exceeds maximum count specified in configuration.
    /// </summary>
    private string? _imageCountExceededError;


    /// <summary>
    ///     Product that will be created.
    /// </summary>
    private ProductInsertModel _product = new();


    /// <summary>
    ///     Category associated with <see cref="_product">product</see> that was selected from dropdown list.
    /// </summary>
    private CategoryModel _selectedCategory = new();


    /// <summary>
    ///     List of all templates present in the database.
    /// </summary>
    private IEnumerable<ProductTemplateModel> _templates = default!;


    /// <summary>
    ///     List of <see cref="_product">product</see> images.
    /// </summary>
    private List<MemoryImageModel> Images => _product.Images;


    [Inject] private IProductData ProductData { get; set; } = default!;
    [Inject] private ICategoryData CategoryData { get; set; } = default!;
    [Inject] private ILogger<CreateProduct> Logger { get; set; } = default!;
    [Inject] private IValidator<ProductInsertModel> Validator { get; set; } = default!;
    [Inject] private IOptionsSnapshot<ImageStorageSettings> Settings { get; set; } = default!;


    public void Dispose()
    {
        GC.Collect();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _categories = await CategoryData.GetLeafCategories();
        _templates = await ProductData.GetAllTemplates();
    }

    /// <summary>
    ///     Inserts new product in the database.
    /// </summary>
    private async Task Create()
    {
        _product.CategoryId = _selectedCategory.Id;
        var isValid = await Validate();
        if (isValid == false) return;


        try
        {
            await ProductData.Create(_product);
            ResetComponent();
        }
        catch (DatabaseException e)
        {
            var exception = new UnreachableException(null, e);
            Logger.LogError(exception, @"Failed to create product. Provided category {Id} 
                    was not present in the database", _product.CategoryId);
            throw exception;
        }
        catch (ValidationException e)
        {
            var exception = new UnreachableException(null, e);
            Logger.LogError(exception, @"Model has failed validation despite the fact that it was
                        successfully validated by {Method}", nameof(Validate));
            throw exception;
        }
        catch (SqlException e)
        {
            _errors.Add(@"There has been a problem a problem while trying to save data. 
                        Try again, if problem persists contact administrator");
            Logger.LogError(e, "There was a problem while trying to save or access data");
        }
    }

    /// <summary>
    ///     Clears component for another insert.
    /// </summary>
    private void ResetComponent()
    {
        _imageCountExceededError = null;
        _errors.Clear();
        _product = new ProductInsertModel();
        _selectedCategory = new CategoryModel();
    }


    /// <summary>
    ///     Loads images provided by the user into the memory.
    /// </summary>
    /// <param name="e">Images to load.</param>
    private async Task LoadImages(InputFileChangeEventArgs e)
    {
        var maxImageCount = Settings.Value.MaximumFileCount;
        var actualImageCount = e.FileCount + Images.Count;
        _imageCountExceededError = null;

        if (actualImageCount > maxImageCount)
        {
            var errorText = $"Maximum amount of pictures is {maxImageCount}, supplied {actualImageCount} images";
            _imageCountExceededError = errorText;
            return;
        }

        var isFirstLoad = Images.Count == 0;

        foreach (var img in e.GetMultipleFiles(maxImageCount))
        {
            var ms = new MemoryStream();
            await img.OpenReadStream(Settings.Value.SizeLimit).CopyToAsync(ms);
            Images.Add(new MemoryImageModel { Name = img.Name, Stream = ms });
        }

        if (isFirstLoad && Images.Count > 0)
            Images[0].IsPrimary = true;
    }


    /// <summary>
    ///     Updates
    /// </summary>
    /// <param name="image"></param>
    private void PrimaryImageChanged(MemoryImageModel image)
    {
        var images = Images.Where(x => x != image);
        foreach (var img in images) img.IsPrimary = false;
    }


    /// <summary>
    ///     Removes specific image from the page.
    /// </summary>
    /// <param name="image">Image to remove.</param>
    private void RemoveImage(MemoryImageModel image)
    {
        Images.Remove(image);
        StateHasChanged();
    }

    /// <summary>
    ///     An event handler that changes keys of <see cref="ProductInsertModel.Properties" /> of the
    ///     <see cref="_product">product</see>.
    /// </summary>
    /// <param name="template">A new template of the <see cref="_product">product</see>.</param>
    /// <remarks>This method will clear all the values from the list even if old and new template share same key.</remarks>
    private void TemplateHasChanged(ProductTemplateModel template)
    {
        _product.Template = template;
        _product.Properties.Clear();
        foreach (var property in template.Properties) _product.Properties.Add(property, new List<string>());
    }


    /// <summary>
    ///     Updates a value of <see cref="ProductInsertModel.Properties">property</see>
    ///     associated with <paramref name="key" /> of the <see cref="_product">product</see>.
    /// </summary>
    /// <param name="key">Key name <see cref="ProductInsertModel.Properties">property</see> of that is changing.</param>
    /// <param name="args">New value.</param>
    private void PropertyChanged(string key, ChangeEventArgs args)
    {
        var text = (string)args.Value!;
        var propertyValue = text.Split(",").ToList();
        _product.Properties[key] = propertyValue;
    }


    /// <summary>
    ///     An event handler that updates <see cref="_categories">categories</see> when child element inserted new item into
    ///     database.
    /// </summary>
    private async Task CategoryDataChanged()
    {
        _categories = await CategoryData.GetLeafCategories();
    }


    /// <summary>
    ///     An event handler that updates <see cref="_templates">templates</see> when child element inserted new item into
    ///     database.
    /// </summary>
    private async Task ProductTemplateChanged()
    {
        _templates = await ProductData.GetAllTemplates();
    }


    /// <summary>
    ///     Clears all images from the page.
    /// </summary>
    private void RemoveAllImages()
    {
        Images.Clear();
    }


    /// <summary>
    ///     Checks if product properties pass all the requirements to be inserted into database.
    /// </summary>
    /// <returns><see langowrd="true" />When product is valid for insert into database. Otherwise <see langword="false" />.</returns>
    private async Task<bool> Validate()
    {
        var validationResult = await Validator.ValidateAsync(_product);

        if (validationResult.IsValid)
            return true;

        foreach (var validationError in validationResult.Errors) _errors.Add(validationError.ErrorMessage);

        return false;
    }
}