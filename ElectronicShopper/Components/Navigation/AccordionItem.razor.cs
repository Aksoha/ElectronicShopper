using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Components.Navigation;

/// <summary>
///     Defines <see cref="Accordion" /> item.
/// </summary>
public partial class AccordionItem : ComponentBase
{
#nullable disable

    /// <inheritdoc cref="Accordion.ChildContent" />
    [Parameter]
    public RenderFragment ChildContent { get; set; }


    /// <summary>
    ///     Determines whether component is visible.
    /// </summary>
    [Parameter]
    public bool Collapsed { get; set; }


    /// <summary>
    ///     The text displayed on accordion header.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter] public string HeaderCssClass { get; set; } = "accordion-header";

    [Parameter] public string ContentCssClass { get; set; } = "accordion-body";
#nullable disable

    /// <summary>
    ///     Toggles visibility state.
    /// </summary>
    private void Toggle()
    {
        Collapsed = !Collapsed;
    }
}