using Bogus;
using ElectronicShopper.Tests.Integration.Extensions;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.Tests.Integration.DataGenerators;

public static class CategoryGenerator
{
    public static CategoryCreateModel Generate()
    {
        var faker = new Faker<CategoryCreateModel>()
            .RuleFor(x => x.Name, x => x.Product().Category());

        return faker;
    }
}