using System.Reflection;
using System.Text.Json;
using ElectronicShopper.Library.Models.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ElectronicShopper.Components.Dropdowns;

/// <summary>
///     Component containing a list of predefined values from which a single value can be chosen.
/// </summary>
/// <typeparam name="T">Specific type of the item.</typeparam>
public partial class DropDownList<T> : ComponentBase, IAsyncDisposable

{
    private IJSObjectReference _module = null!;


    /// <summary>
    ///     Indicates whether component should be rendered.
    /// </summary>
    private bool _shouldRender = true;

    private T? _value;

    /// <summary>
    ///     Source from which dropdown list will be populated.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<T> DataSource { get; set; } = null!;

    /// <summary>
    ///     Name of the property for which values will be displayed in the list.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string PropertyName { get; set; } = null!;

    /// <summary>
    ///     A text when no item has been selected.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    [Parameter] public EventCallback<T> ValueChanged { get; set; }

    /// <summary>
    ///     Time in ms after which component will be collapsed if mouse is not hovering over the component
    /// </summary>
    [Parameter]
    public int CollapseDelay { get; set; } = 400;


    [Parameter] public string DropdownCssClass { get; set; } = "dropdown";

    /// <summary>
    ///     Css class for the header, a place where selected value resides.
    /// </summary>
    [Parameter]
    public string DropdownHeaderCssClass { get; set; } = "dropdown-header";

    /// <summary>
    ///     Css class for the <see cref="Placeholder" />.
    /// </summary>
    [Parameter]
    public string DropdownHeaderPlaceholderCssClass { get; set; } = "dropdown-header-placeholder";

    /// <summary>
    ///     Css class for single item in the list.
    /// </summary>
    [Parameter]
    public string DropdownItemCssClass { get; set; } = "dropdown-item";

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;


    /// <summary>
    ///     Selected value from dropdown.
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

    /// <summary>
    ///     Inline properties for the dropdown items wrapper.
    /// </summary>
    /// <remarks>property "position: fixed" is used to change display behavior when clicking on the header.</remarks>
    private string DropdownItemInlineCss { get; set; } = "";

    /// <summary>
    ///     Determines whether dropdown items are visible.
    /// </summary>
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

    /// <summary>
    ///     Converts given property value to a string.
    /// </summary>
    /// <param name="item">Item to convert.</param>
    /// <exception cref="ArgumentNullException">Thrown when specified <see cref="PropertyName" /> does not exist.</exception>
    private string? ConvertToString(T? item)
    {
        if (item is null)
            return null;

        var property = item.GetType().GetProperty(PropertyName);
        if (property is null)
            throw new ArgumentNullException(nameof(property), "Property with given name does not exist");
        var val = property.GetValue(item, BindingFlags.GetProperty, null, null, null);
        return (string)val!;
    }


    /// <summary>
    ///     Set a current item in a header.
    /// </summary>
    /// <param name="item">Item to set.</param>
    private void SetSelectedItem(T item)
    {
        Value = item;
        Collapse();
    }


    /// <summary>
    ///     Collapses dropdown list.
    /// </summary>
    private void Collapse()
    {
        Expanded = false;
    }


    /// <summary>
    ///     Collapses dropdown list after a <see cref="CollapseDelay">set time</see> has passed.
    /// </summary>
    private async Task CollapseWithDelay()
    {
        await Task.Delay(CollapseDelay);
        Collapse();
    }


    /// <summary>
    ///     Expands dropdown list.
    /// </summary>
    private async Task Expand()
    {
        Expanded = true;
        await SetPopupPosition();
    }

    /// <summary>
    ///     Toggle the <see cref="Expanded" />.
    /// </summary>
    private async Task Toggle()
    {
        if (Expanded)
            Collapse();
        else
            await Expand();
    }

    /// <summary>
    ///     Get position on the screen of the header.
    /// </summary>
    private async Task<RectangleJs> GetBoundingRectangle()
    {
        var result = await _module.InvokeAsync<string>("getDimensions", new object[] { HeaderId });
        var output = JsonSerializer.Deserialize<RectangleJs>(result);
        return output!;
    }

    /// <summary>
    ///     Sets position of the screen of the list.
    /// </summary>
    private async Task SetPopupPosition()
    {
        // turn off rendering otherwise element might be rendered as position:static on first click due to async call in this method
        _shouldRender = false;
        var rectangle = await GetBoundingRectangle();
        DropdownItemInlineCss =
            $"position: fixed; top: {rectangle.Bottom}px; left: {rectangle.Left}px; width: {rectangle.Width}px; z-index: 1000";
        _shouldRender = true;
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }
}