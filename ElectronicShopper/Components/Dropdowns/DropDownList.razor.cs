using System.Reflection;
using System.Text.Json;
using ElectronicShopper.Library.Models.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ElectronicShopper.Components.Dropdowns;

public partial class DropDownList<T> : ComponentBase, IAsyncDisposable

{
    private IJSObjectReference _module = null!;
    private T? _value;
    [Parameter] [EditorRequired] public IEnumerable<T> DataSource { get; set; } = null!;
    [Parameter] [EditorRequired] public string PropertyName { get; set; } = null!;
    [Parameter] public string Placeholder { get; set; } = string.Empty;
    [Parameter] public EventCallback<T> ValueChanged { get; set; }

    /// <summary>
    ///     Time in ms after which component will be collapsed if mouse is not hovering over the component
    /// </summary>
    [Parameter]
    public int CollapseDelay { get; set; } = 400;

    [Parameter] public string DropdownCssClass { get; set; } = "dropdown";
    [Parameter] public string DropdownHeaderCssClass { get; set; } = "dropdown-header";
    [Parameter] public string DropdownHeaderPlaceholderCssClass { get; set; } = "dropdown-header-placeholder";
    [Parameter] public string DropdownItemCssClass { get; set; } = "dropdown-item";
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    
    /// <summary>
    /// Selected value from dropdown
    /// </summary>
    [Parameter]
    public T? Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value))
                return;

            _value = value;
            ValueChanged.InvokeAsync(Value);
        }
    }

    private string HeaderId { get; } = Guid.NewGuid().ToString();
    private string DropdownItemInlineCss { get; set; } = "";
    private bool Expanded { get; set; }


    public async ValueTask DisposeAsync()
    {
        await _module.DisposeAsync();
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
            _module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/ElementDimensions.js");
    }

    private string ConvertToString(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var property = item.GetType().GetProperty(PropertyName);
        if (property is null)
            throw new ArgumentNullException(nameof(property), "Property with given name does not exist");
        var val = property.GetValue(item, BindingFlags.GetProperty, null, null, null);
        return (string)val!;
    }


    private void SetSelectedItem(T item)
    {
        Value = item;
        Collapse();
    }

    private void Collapse()
    {
        Expanded = false;
    }

    private async Task CollapseWithDelay()
    {
        await Task.Delay(CollapseDelay);
        Collapse();
    }

    private async Task Expand()
    {
        Expanded = true;
        await SetPopupPosition();
    }

    private async Task Toggle()
    {
        if (Expanded)
            Collapse();
        else
            await Expand();
    }


    private async Task<RectangleJs> GetBoundingRectangle()
    {
        var result = await _module.InvokeAsync<string>("getDimensions", new object[] { HeaderId });
        var output = JsonSerializer.Deserialize<RectangleJs>(result);
        return output!;
    }

    private async Task SetPopupPosition()
    {
        var rectangle = await GetBoundingRectangle();
        DropdownItemInlineCss =
            $"position: fixed; top: {rectangle.Bottom}px; left: {rectangle.Left}px; width: {rectangle.Width}px; z-index: 1000";
    }
}