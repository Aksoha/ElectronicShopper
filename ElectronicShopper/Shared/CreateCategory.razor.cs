using System.Diagnostics;
using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.Library.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;

namespace ElectronicShopper.Shared;

/// <summary>
///     Component responsible for inserting <see cref="CategoryModel" /> into database.
/// </summary>
[Authorize("admin")]
public partial class CreateCategory : ComponentBase
{
    /// <summary>
    ///     List of all categories.
    /// </summary>
    private IEnumerable<CategoryModel> _categories = default!;


    /// <summary>
    ///     Category to create.
    /// </summary>
    private CategoryCreateModel _category = new();

    /// <summary>
    ///     Parent of <see cref="_category">category to create</see>.
    /// </summary>
    private CategoryModel _parentCategory = new();

    /// <summary>
    ///     Event that is triggered when category is created. This property is required because
    ///     right now <see cref="ICategoryData.GetAll" /> returns a list which will not be updated in entire application.
    ///     Therefor all components have to fetch new data from <see cref="ICategoryData.GetAll" /> manually.
    /// </summary>
    [Parameter]
    public EventCallback CategoryCreated { get; set; }

    [Inject] private ICategoryData CategoryData { get; set; } = default!;

    [Inject] private IValidator<CategoryCreateModel> Validator { get; set; } = default!;

    [Inject] private ILogger<CreateCategory> Logger { get; set; } = default!;

    /// <summary>
    /// List of errors that occured during validation and/or accessing data.
    /// </summary>
    private List<string> _errors = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _categories = await CategoryData.GetAll();
    }


    /// <summary>
    ///     Creates new category and resets component to default state.
    /// </summary>
    private async Task Create()
    {
        _category.ParentId = _parentCategory.Id;
        var isValid = await Validate();
        if (isValid == false) return;

        try
        {
            await CategoryData.Create(_category);
            await CategoryCreated.InvokeAsync();
            _categories = await CategoryData.GetLeafCategories();
            ResetComponent();
        }
        catch (DatabaseException e)
        {
            var exception = new UnreachableException(null, e);
            Logger.LogError(exception, "Failed to save category. ParentId {Id} was not present in the database", _category.ParentId);
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
    ///     Removes selected category from dropdown list.
    /// </summary>
    private void RemoveParentCategory()
    {
        _parentCategory = new CategoryModel();
    }


    /// <summary>
    ///     Clears component for another insert.
    /// </summary>
    private void ResetComponent()
    {
        _category = new CategoryCreateModel();
        _parentCategory = new CategoryModel();
        _errors.Clear();
    }


    /// <summary>
    /// Validates model for save and sets <see cref="_errors">validation errors</see>.
    /// </summary>
    /// <returns><see langword="true"/> when model is valid otherwise <see langword="false"/>.</returns>
    private async Task<bool> Validate()
    {
        _errors.Clear();
        var result = await Validator.ValidateAsync(_category);

        if (result.IsValid)
        {
            return true;
        }

        foreach (var error in result.Errors)
        {
            _errors.Add(error.ErrorMessage);
        }

        return false;
    }
}