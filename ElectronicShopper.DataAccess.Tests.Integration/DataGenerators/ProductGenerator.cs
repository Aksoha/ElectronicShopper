using Bogus;
using ElectronicShopper.DataAccess.Tests.Integration.Extensions;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;

public static class ProductGenerator
{
    public static ProductInsertModel GenerateForInsert()
    {
        var faker = new Faker<ProductInsertModel>()
            .RuleFor(x => x.ProductName, x => x.Commerce.ProductName())
            .RuleFor(x => x.Template, GenerateTemplate());

        var product = faker.Generate();
        foreach (var property in product.Template!.Properties.Select(template => new Product().Property(template)))
        {
            product.Properties.Add(property.Key, property.Value);
        }

        return product;
    }

    public static List<MemoryImageModel> GenerateImages(int num = 2)
    {
        var r = new Random();
        const int maxVal = 100;

        var images = new List<MemoryImageModel>();

        for (var i = 0; i < num; i++)
        {
            var bytes = new byte[r.Next(maxVal)];
            r.NextBytes(bytes);
            var img = new MemoryImageModel
            {
                Name = $"{new Faker().Commerce.ProductName()}.jpg",
                Stream = new MemoryStream(bytes)
            };

            var primaryImage = images.FirstOrDefault();
            if (primaryImage is not null)
                primaryImage.IsPrimary = true;

            images.Add(img);
        }

        return images;
    }

    public static ProductTemplateModel GenerateTemplate(int num = 3)
    {
        var properties = new Faker().Product().UniqueTemplate(num);
        return new ProductTemplateModel { Properties = properties.ToList() };
    }

    public static Dictionary<string, List<string>> GenerateProperties(int num = 3)
    {
        return new Faker().Product().Property(num);
    }
}