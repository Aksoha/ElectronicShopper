using Bogus;
using Bogus.Premium;

namespace ElectronicShopper.DataAccess.Tests.Integration.Extensions;

internal static class FakerProductExtension
{
    public static Product Product(this Faker faker)
    {
        return ContextHelper.GetOrSet(faker, () => new Product());
    }
}