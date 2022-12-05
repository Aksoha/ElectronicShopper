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
            .RuleFor(x => x.Template, GenerateTemplate())
            .RuleFor(x => x.Images, GenerateImages())
            .RuleFor(x => x.Inventory, GenerateRandomInventory(20_000, 500, 15_000));

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
        var name = new Faker().Commerce.Product();
        return new ProductTemplateModel { Properties = properties.ToList(), Name = name };
    }

    public static Dictionary<string, List<string>> GenerateProperties(int num = 3)
    {
        return new Faker().Product().Property(num);
    }

    public static InventoryModel GenerateInventory(int quantity, int reserved, decimal price)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException($"Expected non negative value for " +
                                                  $"parameter {nameof(quantity)}, actual value was {quantity}");
        if (reserved < 0)
            throw new ArgumentOutOfRangeException($"Expected non negative value for " +
                                                  $"parameter {nameof(reserved)}, actual value was {reserved}");
        if (price < 0)
            throw new ArgumentOutOfRangeException($"Expected non negative value for " +
                                                  $"parameter {nameof(price)}, actual value was {price}");
        
        // rounding down to 4 places because that's how many are stored in sql
        price = Math.Round(price, 4);
        return new InventoryModel { Price = price, Quantity = quantity, Reserved = reserved };
    }

    public static InventoryModel GenerateRandomInventory(int maxQuantity, int maxReserved, decimal maxPrice)
    {
        var r = new Random();
        var quantity = r.Next(maxQuantity);
        var reserved = r.Next(maxReserved);
        var price = (decimal)r.NextDouble() * maxPrice;
        return GenerateInventory(quantity, reserved, price);
    }
}