using System.Diagnostics;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;

namespace ElectronicShopper.Shared;

/// <summary>
///     Component responsible for inserting <see cref="ProductTemplateModel" /> into database.
/// </summary>
[Authorize("admin")]
public partial class CreateProductTemplate : ComponentBase
{
    /// <summary>
    ///     List of errors that occured during validation and/or accessing data.
    /// </summary>
    private List<string> _errors = new();

    /// <summary>
    ///     Template to create
    /// </summary>
    private ProductTemplateModel _template = new();

    /// <summary>
    ///     List of all templates
    /// </summary>
    private IEnumerable<ProductTemplateModel> _templates = default!;

    /// <summary>
    ///     Event that is triggered when template is created. This property is required because
    ///     right now <see cref="IProductData.GetAllTemplates" /> returns a list which will not be updated in entire
    ///     application.
    ///     Therefor all components have to fetch new data from <see cref="IProductData.GetAllTemplates" /> manually.
    /// </summary>
    [Parameter]
    public EventCallback TemplateCreated { get; set; }

    [Inject] private IProductData ProductData { get; set; } = default!;

    [Inject] private IValidator<ProductTemplateModel> Validator { get; set; } = default!;

    [Inject] private ILogger<CreateProductTemplate> Logger { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _templates = await ProductData.GetAllTemplates();
    }

    /// <summary>
    ///     Creates new template and resets component to default state.
    /// </summary>
    private async Task Create()
    {
        var isValid = await Validate();
        if (isValid == false) return;

        try
        {
            await ProductData.CreateTemplate(_template);
            await TemplateCreated.InvokeAsync();
            _templates = await ProductData.GetAllTemplates();
            ResetComponent();
        }
        catch (ValidationException e)
        {
            var exception = new UnreachableException(null, e);
            Logger.LogError(exception, @"Model has failed validation despite the fact that it was 
                    successfully validated by {Method}", nameof(Validate));
            throw;
        }
        catch (SqlException e)
        {
            _errors.Add(@"There has been a problem a problem while trying to save data. 
                        Try again, if problem persists contact administrator");
            Logger.LogError(e, "There was a problem while trying to save or access data");
        }
    }


    /// <summary>
    ///     Adds empty property to <see cref="_template" />.
    /// </summary>
    private void AddProperty()
    {
        _template.Properties.Add("");
    }

    /// <summary>
    ///     Updates property.
    /// </summary>
    /// <param name="property">Old property name.</param>
    /// <param name="args">New property name.</param>
    private void UpdateProperty(string property, ChangeEventArgs args)
    {
        var text = (string)args.Value!;
        var index = _template.Properties.IndexOf(property);
        if (index == -1)
            return;
        _template.Properties[index] = text;
        StateHasChanged();
    }


    /// <summary>
    ///     Removes property.
    /// </summary>
    /// <param name="property">Property to remove.</param>
    private void RemoveProperty(string property)
    {
        _template.Properties.Remove(property);
        StateHasChanged();
    }

    /// <summary>
    ///     Changes <see cref="ProductTemplateModel.Properties" /> to match those of <paramref name="template" />.
    ///     Calling this method will clear properties including custom ones that do not come from <paramref name="template" />.
    /// </summary>
    /// <param name="template">New template of a template.</param>
    private void BasedOnChanged(ProductTemplateModel template)
    {
        _template = new ProductTemplateModel
        {
            Properties = template.Properties
        };
    }

    /// <summary>
    ///     Clears component for another insert.
    /// </summary>
    private void ResetComponent()
    {
        _template = new ProductTemplateModel();
        _errors.Clear();
    }


    /// <summary>
    ///     Validates model for save and sets <see cref="_errors">validation errors</see>.
    /// </summary>
    /// <returns><see langword="true" /> when model is valid otherwise <see langword="false" />.</returns>
    private async Task<bool> Validate()
    {
        _errors.Clear();
        var result = await Validator.ValidateAsync(_template);

        if (result.IsValid) return true;

        foreach (var error in result.Errors) _errors.Add(error.ErrorMessage);

        return false;
    }
}