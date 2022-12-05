using Bogus;
using Bogus.Premium;

namespace ElectronicShopper.Tests.Integration.Extensions;

internal static class FakerProductExtension
{
    public static Product Product(this Faker faker)
    {
        return ContextHelper.GetOrSet(faker, () => new Product());
    }
}