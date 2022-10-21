using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.DataAccess.Identity;

public sealed class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<ApplicationUserManager> logger) : base(
        store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services,
        logger)
    {
        RegisterTokenProvider(TokenOptions.DefaultProvider, new EmailTokenProvider<ApplicationUser>());
    }
}