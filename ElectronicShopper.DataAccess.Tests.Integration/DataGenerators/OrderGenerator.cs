using Bogus;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;

public static class OrderGenerator
{
    public static OrderModel Generate()
    {
        var faker = new Faker<OrderModel>()
            .RuleFor(x => x.UserId, x => x.Random.Number(2000));

        return faker;
    }

    public static List<OrderDetailModel> OrderDetails(IEnumerable<ProductInsertModel> product, int quantity = 50)
    {
        var r = new Random();
        return product.Select(item => new OrderDetailModel
            { Quantity = quantity, ProductId = item.Id, PricePerItem = r.Next(1000) }).ToList();
    }
}