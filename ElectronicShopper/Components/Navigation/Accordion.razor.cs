using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Components.Navigation;

/// <summary>
///     Component with vertically collapsable panel that displays one or more panels at a time.
/// </summary>
public partial class Accordion : ComponentBase
{
#nullable disable

    /// <summary>
    ///     The child component for accordion.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    ///     Css class for accordion customization.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = "accordion";
#nullable enable
}