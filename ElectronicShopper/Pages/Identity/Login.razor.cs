using ElectronicShopper.Library;
using ElectronicShopper.Library.Identity;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Middleware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Pages.Identity;

/// <summary>
///     Login page.
/// </summary>
public partial class Login : ComponentBase
{
    private const string ForgotPasswordUrl = "/register";


    private readonly LoginModel _loginModel = new() { RememberMe = true };

    /// <summary>
    ///     error message displayed when authentication failed.
    /// </summary>
    private string _errorMessage = "";

    [Inject] private Navigation NavManager { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;

    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var user = (await AuthState).User;
        if (user.Identity is not null && user.Identity.IsAuthenticated) NavManager.NavigateTo("/");
    }

    /// <summary>
    ///     Attempts to authenticate user.
    /// </summary>
    /// <exception cref="NotImplementedException">Thrown when user had 2FA enabled.</exception>
    private async Task Authenticate()
    {
        _errorMessage = "";

        var user = await UserManager.FindByEmailAsync(_loginModel.Email);
        if (user is null)
        {
            _errorMessage = "Incorrect email and/or password";
            return;
        }

        if (await SignInManager.CanSignInAsync(user))
        {
            var result = await SignInManager.CheckPasswordSignInAsync(user, _loginModel.Password, false);
            if (result.Succeeded)
                AuthorizationMiddleware.RequestLogin(_loginModel, NavManager);
            else if (result.IsLockedOut)
                _errorMessage = "Your account has been blocked";
            else if (result.RequiresTwoFactor)
                throw new NotImplementedException();
            else
                _errorMessage = "Incorrect email and/or password";
        }
    }
}