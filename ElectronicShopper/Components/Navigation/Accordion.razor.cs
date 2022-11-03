using Microsoft.AspNetCore.Components;

namespace ElectronicShopper.Components.Navigation;

public partial class Accordion : ComponentBase
{
#nullable disable
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] public string CssClass { get; set; } = "accordion";
#nullable enable
}