using System.Collections.Concurrent;
using ElectronicShopper.DataAccess.Identity;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Models;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Middleware;

public class AuthorizationMiddleware
{
    private static IDictionary<Guid, LoginModel> Logins { get; set; }
        = new ConcurrentDictionary<Guid, LoginModel>();

    private readonly RequestDelegate _next;

    private const string LoginUrl = "/login";
    private const string LogoutUrl = "/logout";
    
    /// <summary>
    /// Name of a key that has to be passed in the <see cref="HttpContext"/> in order to intercept a call.
    /// </summary>
    private const string KeyName = "key";

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
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
            await Login(context, signInManager, userManager);
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

    private static async Task Login(HttpContext context, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        var key = Guid.Parse(context.Request.Query[KeyName]);
        var info = Logins[key];

        var user = await userManager.FindByEmailAsync(info.Email);
        if (user is null)
        {
            RedirectToLoginIncorrectCredentials(context);
            return;
        }

        var validCredentials = await signInManager.UserManager.CheckPasswordAsync(user, info.Password);
        if (validCredentials == false)
        {
            RedirectToLoginIncorrectCredentials(context);
            return;
        }

        var result =
            await signInManager.PasswordSignInAsync(info.Email, info.Password, info.RememberMe, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            Logins.Remove(key);
            context.Response.Redirect("/");
        }
        else if (result.RequiresTwoFactor)
        {
            throw new NotImplementedException();
        }
        else if (result.IsLockedOut)
        {
            const string errorMessage = "Your account has been blocked";
            context.Response.Redirect($"{LoginUrl}?errorMessage={errorMessage}");
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

    private static void RedirectToLoginIncorrectCredentials(HttpContext context)
    {
        const string errorMessage = "Incorrect email and/or password";
        context.Response.Redirect($"{LoginUrl}?errorMessage={errorMessage}");
    }
}