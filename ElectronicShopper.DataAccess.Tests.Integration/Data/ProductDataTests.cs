using System.Text.Json;
using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;
using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.DataAccess.Tests.Integration.Data;

[Collection("database collection")]
public class ProductDataTests : IAsyncLifetime
{
    private readonly CategoryData _categoryData;
    private readonly ProductData _productData;
    private readonly Func<Task> _resetDatabase;

    public ProductDataTests(DatabaseFactory dbFactory)
    {
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


    [Fact]
    public async Task Create_WhenDataIsValid()
    {
        // Arrange
        var p = ProductGenerator.GenerateForInsert();
        var c = CategoryGenerator.Generate();
        await _categoryData.Create(c);
        p.CategoryId = c.Id;


        // Act
        await _productData.Create(p);


        // Assert
        Assert.NotNull(p.Id);
    }

    [Fact]
    public async Task Create_WhenCategoryIdIsNull()
    {
        // Arrange
        var p = ProductGenerator.GenerateForInsert();
        var c = CategoryGenerator.Generate();
        await _categoryData.Create(c);
        p.CategoryId = null;


        // Act
        async Task Act()
        {
            await _productData.Create(p);
        }


        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }

    [Fact]
    public async Task Create_WhenCategoryIsNotInTheDatabase()
    {
        // Arrange
        var p = ProductGenerator.GenerateForInsert();
        p.CategoryId = Random.Shared.Next();


        // Act
        async Task Act()
        {
            await _productData.Create(p);
        }


        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WhenProductNameIsEmptyOrWhitespace(string productName)
    {
        // Arrange
        var p = ProductGenerator.GenerateForInsert();
        var c = CategoryGenerator.Generate();
        await _categoryData.Create(c);
        p.CategoryId = c.Id;
        p.ProductName = productName;


        // Act
        async Task Act()
        {
            await _productData.Create(p);
        }


        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }


    [Fact]
    public async Task CreateTemplate_WhenDataIsValid()
    {
        // Arrange
        var template = ProductGenerator.GenerateTemplate();


        // Act
        await _productData.CreateTemplate(template);


        // Assert
        Assert.NotNull(template.Id);
    }


    [Theory]
    [InlineData("Concrete Steel Bike.jpg")]
    [InlineData("Awesome Rubber Car.jpeg")]
    [InlineData("Practical Steel Pizza.png")]
    [InlineData("Gorgeous Wooden Pants.svg")]
    public async Task CreateImage_WhenDataIsValid(string filename)
    {
        // Arrange
        var product = await CreateProductWithDependencies();
        var img = ProductGenerator.GenerateImages(1).Single();
        img.Name = filename;


        // Act
        await _productData.CreateImage(product, img);


        // Assert
        Assert.NotNull(img.Id);
    }

    [Fact]
    public async Task CreateImage_WhenImageNameIsEmpty()
    {
        // Arrange
        var product = await CreateProductWithDependencies();
        var img = ProductGenerator.GenerateImages(1).Single();
        img.Name = ".jpg";


        // Act
        async Task Act()
        {
            await _productData.CreateImage(product, img);
        }


        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }

    [Theory]
    [InlineData("Concrete Steel Bike.pdf")]
    [InlineData("Gorgeous Wooden Pants.tiff")]
    public async Task CreateImage_WhenImageExtensionIsNotSupported(string extension)
    {
        // Arrange
        var product = await CreateProductWithDependencies();
        var img = ProductGenerator.GenerateImages(1).Single();
        img.Name = extension;


        // Act
        async Task Act()
        {
            await _productData.CreateImage(product, img);
        }


        // Assert
        await Assert.ThrowsAsync<ValidationException>(Act);
    }


    [Fact]
    public async Task Get()
    {
        // Arrange
        var p = ProductGenerator.GenerateForInsert();
        var c = CategoryGenerator.Generate();
        await _categoryData.Create(c);
        p.CategoryId = c.Id;
        await _productData.Create(p);

        var images = ProductGenerator.GenerateImages();
        foreach (var img in images) await _productData.CreateImage(new ProductModel { Id = p.Id }, img);

        var r = new Random();
        var expectedInventory = await CreateInventory(p, r.Next(2_000), r.Next(50), r.Next(1_000));


        // Act
        var actual = await _productData.Get((int)p.Id!);


        // Assert
        Assert.NotNull(actual);
        Assert.Equal(p.Id, actual!.Id);
        Assert.Equal(p.CategoryId, actual.Category.Id);
        Assert.Equal(expectedInventory.Id, actual.Inventory.Id);
        Assert.Equal(expectedInventory.Price, actual.Inventory.Price);
        Assert.Equal(expectedInventory.Quantity, actual.Inventory.Quantity);
        Assert.Equal(expectedInventory.Reserved, actual.Inventory.Reserved);
        Assert.Equal(p.ProductName, actual.ProductName);
        Assert.Equal(images.Count, actual.Images.Count);
        for (var i = 0; i < actual.Images.Count; i++)
        {
            var expectedImage = images[i];
            var actualImage = actual.Images[i];
            Assert.Equal(expectedImage.Id, actualImage.Id);
            Assert.Equal(expectedImage.Extension, actualImage.Extension);
            Assert.Equal(expectedImage.Name, actualImage.Name);
            Assert.Equal(expectedImage.IsPrimary, actualImage.IsPrimary);
        }

        var expectedProperties = JsonSerializer.Serialize(p.Properties);
        var actualProperties = JsonSerializer.Serialize(actual.Properties);
        Assert.Equal(expectedProperties, actualProperties);
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var createdProducts = new List<ProductModel>();
        for (var i = 0; i < 20; i++)
        {
            var r = new Random();
            var product = await CreateProductWithDependencies();
            product.Inventory = await CreateInventory(product, r.Next(), r.Next(), r.Next());
            createdProducts.Add(product);
        }


        // Act
        var result = await _productData.GetAll();


        // Assert
        Assert.True(result.Count >= createdProducts.Count);
        foreach (var actual in result)
        {
            Assert.NotNull(actual.Id);
            var expected = createdProducts.Single(x => x.Id == actual.Id);
            Assert.Equal(expected.Category.Id, actual.Category.Id);
            Assert.Equal(expected.Inventory.Id, actual.Inventory.Id);
            Assert.Equal(expected.Inventory.Price, actual.Inventory.Price);
            Assert.Equal(expected.Inventory.Quantity, actual.Inventory.Quantity);
            Assert.Equal(expected.Inventory.Reserved, actual.Inventory.Reserved);
            Assert.Equal(expected.ProductName, actual.ProductName);
            Assert.Equal(expected.Images.Count, actual.Images.Count);
            for (var i = 0; i < actual.Images.Count; i++)
            {
                var expectedImage = expected.Images[i];
                var actualImage = actual.Images[i];
                Assert.Equal(expectedImage.Id, actualImage.Id);
                Assert.Equal(expectedImage.Extension, actualImage.Extension);
                Assert.Equal(expectedImage.Name, actualImage.Name);
                Assert.Equal(expectedImage.IsPrimary, actualImage.IsPrimary);
            }

            var expectedProperties = JsonSerializer.Serialize(expected.Properties);
            var actualProperties = JsonSerializer.Serialize(actual.Properties);
            Assert.Equal(expectedProperties, actualProperties);
        }
    }

    [Fact]
    public async Task GetTemplate()
    {
        // Arrange
        var expected = ProductGenerator.GenerateTemplate();
        await _productData.CreateTemplate(expected);


        // Act
        var actual = await _productData.GetTemplate((int)expected.Id!);


        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual!.Id);
        var expectedProperties = JsonSerializer.Serialize(expected.Properties);
        var actualProperties = JsonSerializer.Serialize(actual.Properties);
        Assert.Equal(expectedProperties, actualProperties);
    }

    [Fact]
    public async Task GetAllTemplates()
    {
        // Arrange
        var createdTemplates = new List<ProductTemplateModel>();
        for (var i = 0; i < 20; i++)
        {
            var template = ProductGenerator.GenerateTemplate();
            await _productData.CreateTemplate(template);
            createdTemplates.Add(template);
        }


        // Act
        var result = await _productData.GetAllTemplates();


        // Assert
        Assert.True(result.Count >= createdTemplates.Count);
        foreach (var actual in result)
        {
            Assert.NotNull(actual.Id);
            var expected = createdTemplates.Single(x => x.Id == actual.Id);
            var expectedProperties = JsonSerializer.Serialize(expected.Properties);
            var actualProperties = JsonSerializer.Serialize(actual.Properties);
            Assert.Equal(expectedProperties, actualProperties);
        }
    }

    [Fact]
    public async Task GetProductImages()
    {
        // Arrange
        var product = await CreateProductWithDependencies();
        var images = ProductGenerator.GenerateImages();

        foreach (var img in images) await _productData.CreateImage(product, img);


        // Act
        var result = await _productData.GetProductImages(product);

        Assert.True(images.Count == result.Count);
        foreach (var actual in result)
        {
            Assert.NotNull(actual.Id);
            var expected = images.Single(x => x.Id == actual.Id);
            Assert.Equal(expected.Extension, actual.Extension);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.IsPrimary, actual.IsPrimary);
        }
    }


    [Fact]
    public async Task UpdateInventory_WhenDataIsValid()
    {
        // Arrange
        var product = await CreateProductWithDependencies();
        var oldInventory = await CreateInventory(product, 1, 2, (decimal)3.33);
        var expected = new InventoryModel { Price = (decimal)2.12, Quantity = 14, Reserved = 1 };

        // Act
        await _productData.UpdateInventory(product, expected);
        var actual = (await _productData.Get((int)product.Id!))!.Inventory;


        // Assert
        Assert.Same(expected, product.Inventory);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Price, actual.Price);
        Assert.Equal(expected.Quantity, actual.Quantity);
        Assert.Equal(expected.Reserved, actual.Reserved);
    }


    [Fact]
    public async Task UpdateInventory_WhenProductIdIsNull()
    {
        // Arrange
        var p = new ProductModel();
        var expected = new InventoryModel { Price = (decimal)2.12, Quantity = 14, Reserved = 1 };


        // Act
        async Task Act()
        {
            await _productData.UpdateInventory(p, expected);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }


    [Fact]
    public async Task UpdateInventory_WhenProductIsNotInTheDatabase()
    {
        // Arrange
        var p = new ProductModel { Id = Random.Shared.Next() };
        var expected = new InventoryModel { Price = (decimal)2.12, Quantity = 14, Reserved = 1 };


        // Act
        async Task Act()
        {
            await _productData.UpdateInventory(p, expected);
        }

        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }


    /// <summary>
    ///     Creates and saves product with random properties into the database.
    /// </summary>
    /// <returns>Newly created <see cref="ProductModel" /></returns>
    /// <remarks>
    ///     Does not set <see cref="ProductModel.Inventory" />,
    ///     <see cref="ProductModel.Images" />, <see cref="ProductModel.Discontinued" /> properties.
    /// </remarks>
    private async Task<ProductModel> CreateProductWithDependencies()
    {
        var p = ProductGenerator.GenerateForInsert();
        var c = CategoryGenerator.Generate();
        await _categoryData.Create(c);
        p.CategoryId = c.Id;
        await _productData.Create(p);
        var category = await _categoryData.Get((int)c.Id!);

        return new ProductModel
            { Id = p.Id, Category = category!, ProductName = p.ProductName, Properties = p.Properties };
    }

    private async Task<InventoryModel> CreateInventory(IDbEntity product, int quantity, int reserved,
        decimal price)
    {
        ArgumentNullException.ThrowIfNull(product.Id);

        var inventory = new InventoryModel
        {
            Price = price,
            Quantity = quantity,
            Reserved = reserved
        };

        await _productData.CreateInventory((int)product.Id, inventory);

        return inventory;
    }
}