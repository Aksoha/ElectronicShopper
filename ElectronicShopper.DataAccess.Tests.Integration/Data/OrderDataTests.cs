using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;
using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.DataAccess.Tests.Integration.Data;

[Collection("database collection")]
public class OrderDataTests : IAsyncLifetime
{
    private readonly CategoryData _categoryData;
    private readonly OrderData _orderData;
    private readonly ProductData _productData;
    private readonly Func<Task> _resetDatabase;


    public OrderDataTests(DatabaseFactory dbFactory)
    {
        _orderData = dbFactory.OrderData;
        _productData = dbFactory.ProductData;
        _categoryData = dbFactory.CategoryData;
        _resetDatabase = dbFactory.ResetDatabase;
    }


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }

    [Theory]
    [InlineData(10, 0, 2)]
    [InlineData(15, 0, 15)]
    [InlineData(0, 32, 2)]
    [InlineData(0, 12, 12)]
    [InlineData(5, 2, 7)]
    [InlineData(2, 3, 5)]
    public async Task Create_WhenDataIsValid(int inventoryQuantity, int inventoryReserve, int purchaseQuantity)
    {
        // Arrange
        var productList = await GenerateProducts();
        var order = OrderGenerator.Generate();
        order.PurchasedProducts = OrderGenerator.OrderDetails(productList, purchaseQuantity);

        foreach (var item in productList)
            await CreateInventory(item, inventoryQuantity, inventoryReserve, 1);

        // Act
        await _orderData.Create(order);


        // Assert
        Assert.NotNull(order.Id);
        Assert.Equal(DateTime.UtcNow, order.PurchaseTime, TimeSpan.FromMinutes(1));
        foreach (var item in order.PurchasedProducts) Assert.NotNull(item.Id);
    }

    [Theory]
    [InlineData(10, 0, 12)]
    [InlineData(0, 2, 23)]
    [InlineData(5, 2, 22)]
    [InlineData(2, 3, 10)]
    public async Task Create_WhenInventoryQuantityIsTooLow(int inventoryQuantity, int inventoryReserve,
        int purchaseQuantity)
    {
        // Arrange
        var productList = await GenerateProducts();
        var order = OrderGenerator.Generate();
        order.PurchasedProducts = OrderGenerator.OrderDetails(productList, purchaseQuantity);

        foreach (var item in productList)
            await CreateInventory(item, inventoryQuantity, inventoryReserve, 1);

        // Act
        async Task Act()
        {
            await _orderData.Create(order);
        }


        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }

    [Fact]
    public async Task Create_WhenPurchasesProductsCountIsZero()
    {
        // Arrange
        var order = OrderGenerator.Generate();

        // Act
        async Task Act()
        {
            await _orderData.Create(order);
        }

        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Create_WhenQuantityIsNotPositive(int quantity)
    {
        // Arrange
        var productList = await GenerateProducts(1);
        var order = OrderGenerator.Generate();
        order.PurchasedProducts = OrderGenerator.OrderDetails(productList);

        foreach (var item in order.PurchasedProducts) item.Quantity = quantity;

        // Act
        async Task Act()
        {
            await _orderData.Create(order);
        }

        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }

    [Fact]
    public async Task Create_WhenProductIdIsNull()
    {
        // Arrange
        var productList = await GenerateProducts(1);
        var order = OrderGenerator.Generate();
        order.PurchasedProducts = OrderGenerator.OrderDetails(productList);

        foreach (var item in order.PurchasedProducts) item.ProductId = null;

        // Act
        async Task Act()
        {
            await _orderData.Create(order);
        }


        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }

    [Fact]
    public async Task Create_WhenPricePerItemIsNegative()
    {
        // Arrange
        var productList = await GenerateProducts(1);
        var order = OrderGenerator.Generate();
        order.PurchasedProducts = OrderGenerator.OrderDetails(productList);

        foreach (var item in order.PurchasedProducts) item.PricePerItem = -1;

        // Act
        async Task Act()
        {
            await _orderData.Create(order);
        }

        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }

    private async Task<List<ProductInsertModel>> GenerateProducts(int num = 3)
    {
        var productList = new List<ProductInsertModel>();
        for (var i = 0; i < num; i++)
        {
            var p = ProductGenerator.GenerateForInsert();
            var c = CategoryGenerator.Generate();
            await _categoryData.Create(c);
            p.CategoryId = c.Id;
            await _productData.Create(p);
            productList.Add(p);
        }

        return productList;
    }

    private async Task CreateInventory(ProductInsertModel product, int quantity, int reserved, decimal price)
    {
        ArgumentNullException.ThrowIfNull(product.Id);

        var inventory = new InventoryModel
        {
            Price = price,
            Quantity = quantity,
            Reserved = reserved
        };

        await _productData.CreateInventory((int)product.Id, inventory);
    }
}