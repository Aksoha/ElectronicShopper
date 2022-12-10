using System.Security.Claims;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Identity;
using ElectronicShopper.Library.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Shared;

/// <summary>
///     A component that is displayed on top of all pages. Contains login/register and cart.
/// </summary>
public partial class AppBar : ComponentBase, IDisposable
{
    [Inject] private ICartService CartService { get; set; } = default!;
    [Inject] private Navigation NavManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;

    public void Dispose()
    {
        CartService.CartCountChange -= UpdateCartIcon;
    }


    protected override Task OnInitializedAsync()
    {
        CartService.CartCountChange += UpdateCartIcon;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Redirects to cart page.
    /// </summary>
    private void OnCartClick()
    {
        NavManager.NavigateTo("/cart");
    }


    /// <summary>
    /// Updates the cart item count.
    /// </summary>
    private void UpdateCartIcon()
    {
        InvokeAsync(StateHasChanged);
    }


    private async Task<string?> GetUserName(ClaimsPrincipal user)
    {
        var u = await UserManager.GetUserAsync(user);
        return u?.FirstName;
    }
}