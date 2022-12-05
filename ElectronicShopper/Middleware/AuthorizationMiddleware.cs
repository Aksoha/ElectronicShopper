using System.Collections.Concurrent;
using System.Security.Authentication;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Identity;
using ElectronicShopper.Library.Models;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Middleware;

public class AuthorizationMiddleware
{
    private static IDictionary<Guid, LoginModel> Logins { get; set; }
        = new ConcurrentDictionary<Guid, LoginModel>();

    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    private const string LoginUrl = "/login";
    private const string LogoutUrl = "/logout";

    /// <summary>
    /// Name of a key that has to be passed in the <see cref="HttpContext"/> in order to intercept a call.
    /// </summary>
    private const string KeyName = "key";

    public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Attempts to authorize user.
    /// </summary>
    /// <param name="login">Information of the user attempting to login</param>
    /// <param name="navigation">Navigation</param>
    /// <remarks>
    /// This operation will cause redirection to other page.
    /// </remarks>
    public static void RequestLogin(LoginModel login, Navigation navigation)
    {
        var key = Guid.NewGuid();
        Logins[key] = login;
        navigation.NavigateTo($"{LoginUrl}?{KeyName}={key}", true);
    }


    public async Task Invoke(HttpContext context, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        if (context.Request.Path == LoginUrl && context.Request.Query.ContainsKey(KeyName))
        {
            await Login(context, signInManager);
        }
        else if (context.Request.Path == LogoutUrl)
        {
            await Logout(context, signInManager);
        }
        else
        {
            await _next.Invoke(context);
        }
    }

    private async Task Login(HttpContext context, SignInManager<ApplicationUser> signInManager)
    {
        var key = Guid.Parse(context.Request.Query[KeyName]);
        var info = Logins[key];
        
        var result =
            await signInManager.PasswordSignInAsync(info.Email, info.Password, info.RememberMe, lockoutOnFailure: true);

        if (result == SignInResult.Failed)
        {
            _logger.LogError(new AuthenticationException(), "Authentication has failed");
        }
        
        if (result.Succeeded)
        {
            Logins.Remove(key);
            context.Response.Redirect("/");
        }
        else if (result.RequiresTwoFactor)
        {
            var ex = new NotImplementedException();
            _logger.LogError(ex, "Attempted to use unsupported feature");
            throw ex;
        }
        else if (result.IsLockedOut)
        {
            _logger.LogError(new AuthenticationException(), "Provided incorrect user credentials");
            context.Response.Redirect("/");
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private static async Task Logout(HttpContext context, SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        context.Response.Redirect("/");
    }
    
}