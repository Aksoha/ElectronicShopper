using Bogus;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.Tests.Integration.DataGenerators;

public static class OrderGenerator
{
    public static OrderModel Generate()
    {
        var faker = new Faker<OrderModel>()
            .RuleFor(x => x.UserId, x => x.Random.Number(2000));

        return faker;
    }

    public static List<OrderDetailModel> OrderDetails(IEnumerable<int?> productId, int quantity = 50)
    {
        var r = new Random();
        return productId.Select(item => new OrderDetailModel
            { Quantity = quantity, ProductId = item, PricePerItem = r.Next(1000) }).ToList();
    }
}