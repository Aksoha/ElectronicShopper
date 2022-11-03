using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Components.Navigation;

public partial class AccordionItem : ComponentBase
{
#nullable disable
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public bool Collapsed { get; set; }

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string HeaderCssClass { get; set; } = "accordion-header";

    [Parameter]
    public string ContentCssClass { get; set; } = "accordion-body";
#nullable disable
    private void Toggle()
    {
        Collapsed = !Collapsed;
    }
}