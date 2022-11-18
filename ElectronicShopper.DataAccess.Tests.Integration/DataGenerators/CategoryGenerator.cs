using Bogus;
using ElectronicShopper.DataAccess.Tests.Integration.Extensions;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;

public static class CategoryGenerator
{
    public static CategoryCreateModel Generate()
    {
        var faker = new Faker<CategoryCreateModel>()
            .RuleFor(x => x.Name, x => x.Product().Category());

        return faker;
    }
}