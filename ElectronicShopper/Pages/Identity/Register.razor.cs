using ElectronicShopper.Library.Identity;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Middleware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Pages.Identity;

/// <summary>
///     Register page.
/// </summary>
public partial class Register : ComponentBase
{
    private const string LoginUrl = "/login";
    private readonly RegisterModel _registerModel = new();
    private string _errorMessage = "";

    [Inject] private IUserStore<ApplicationUser> UserStore { get; set; } = default!;
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; } = default!;
    [Inject] private IUserEmailStore<IdentityUser> EmailStore { get; set; } = default!;

    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var user = (await AuthState).User;
        if (user.Identity is not null && user.Identity.IsAuthenticated) NavManager.NavigateTo("/");
    }

    /// <summary>
    ///     Attempts to register new user.
    /// </summary>
    private async Task TryToRegister()
    {
        _errorMessage = "";

        var user = await UserManager.FindByEmailAsync(_registerModel.Email);
        if (user is not null)
        {
            _errorMessage = "Email is already taken";
            return;
        }

        await RegisterUser();
    }

    /// <summary>
    ///     Register new user.
    /// </summary>
    private async Task RegisterUser()
    {
        var emailStore = (IUserEmailStore<ApplicationUser>)UserStore;

        var user = new ApplicationUser
        {
            FirstName = _registerModel.FirstName!,
            LastName = _registerModel.LastName!,
            RegistrationTime = DateTime.UtcNow
        };
        await UserStore.SetUserNameAsync(user, _registerModel.Email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, _registerModel.Email, CancellationToken.None);
        var result = await UserManager.CreateAsync(user, _registerModel.Password);

        if (result.Succeeded)
        {
            // TODO: add confirm registration uri
            // var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // HACK: no mailing system as of now, all accounts will be auto confirmed
            await emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);

            AuthorizationMiddleware.RequestLogin(
                new LoginModel { Email = _registerModel.Email, Password = _registerModel.Password, RememberMe = true },
                NavManager);
        }
    }
}