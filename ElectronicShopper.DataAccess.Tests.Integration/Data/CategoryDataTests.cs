using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.DataAccess.Tests.Integration.DataGenerators;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Tests.Integration.Data;

[Collection("database collection")]
public class CategoryDataTests : IAsyncLifetime
{
    private readonly CategoryData _data;
    private readonly Func<Task> _resetDatabase;

    public CategoryDataTests(DatabaseFactory dbFactory)
    {
        _data = dbFactory.CategoryData;
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
        var category = CategoryGenerator.Generate();

        // Act
        await _data.Create(category);

        // Assert
        Assert.NotNull(category.Id);
    }

    [Fact]
    public async Task Create_WhenParentIsNotPresent()
    {
        // Arrange
        var category = CategoryGenerator.Generate();
        category.ParentId = Random.Shared.Next();

        // Act
        async Task Act()
        {
            await _data.Create(category);
        }


        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }


    [Fact]
    public async Task Get_WhenDataIsValid()
    {
        // Arrange
        var category = CategoryGenerator.Generate();
        var parent = CategoryGenerator.Generate();
        var parentParent = CategoryGenerator.Generate();

        await _data.Create(parentParent);
        parent.ParentId = parentParent.Id;
        await _data.Create(parent);
        category.ParentId = parent.Id;
        await _data.Create(category);


        // Act
        var actual = await _data.Get((int)category.Id!);

        // Assert
        Assert.Equal(category.Id, actual!.Id);
        Assert.Equal(category.Name, actual.Name);
        Assert.Equal(parent.Id, actual.Parent!.Id);
        Assert.Equal(parent.Name, actual.Parent!.Name);
        Assert.Equal(parentParent.Id, actual.Parent!.Parent!.Id);
        Assert.Equal(parentParent.Name, actual.Parent!.Parent.Name);
    }


    [Fact]
    public async Task Get_WhenCategoryIsNotPresent()
    {
        // Act
        var actual = await _data.Get(Random.Shared.Next());

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetRootCategories()
    {
        // Arrange
        const int count = 20;
        var rootCategories = new List<CategoryModel>();
        for (var i = 0; i < count; i++)
        {
            var category = CategoryGenerator.Generate();
            await _data.Create(category);
            rootCategories.Add(new CategoryModel { Id = category.Id, Name = category.Name });
        }

        var leafCategories = new List<CategoryModel>();
        for (var i = 0; i < Random.Shared.Next(count); i++)
        {
            var category = CategoryGenerator.Generate();
            category.ParentId = rootCategories[i].Id;
            await _data.Create(category);
            leafCategories.Add(new CategoryModel { Id = category.Id, Name = category.Name });
        }


        // Act
        var expected = rootCategories.Count;
        var result = await _data.GetRootCategories();

        
        // Assert
        var actual = rootCategories.Select(x => x.Id)
            .Intersect(
                result.Select(x => x.Id)).Count();
        Assert.Equal(expected, actual);
        Assert.True(result.Count(x => x.Parent is null) == expected);
        
        var leafItemsInResult = leafCategories.
            Select(x => x.Id)
            .Intersect(
                result.Select(x => x.Id)).Count();
        Assert.Equal(0, leafItemsInResult);
    }

    
    [Fact]
    public async Task Update_WhenDataIsValid()
    {
        // Assert
        var c1 = CategoryGenerator.Generate();
        var c2 = CategoryGenerator.Generate();
        
        await _data.Create(c1);
        await _data.Create(c2);
        
        var oldCategory = new CategoryModel { Id = c1.Id, Name = c1.Name };
        var newCategory = new CategoryModel { Id = c2.Id, Name = c2.Name };
        
        // Act
        await _data.Update(oldCategory, newCategory);
        var actual = await _data.Get((int)oldCategory.Id!);
        
        // Assert
        Assert.Equal(newCategory.Id, actual!.Id);
        Assert.Equal(newCategory.Name, actual.Name);
    }

    [Fact]
    public async Task Update_WhenParentIsNotPresent()
    {
        // Arrange
        var c = CategoryGenerator.Generate();
        
        await _data.Create(c);
        
        var oldCategory = new CategoryModel { Id = c.Id, Name = c.Name };
        var newCategory = new CategoryModel { Parent = new CategoryModel {Id = Random.Shared.Next()}};
        
        
        // Act
        async Task Act()
        {
            await _data.Update(oldCategory, newCategory);
        }
    
        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }


    [Fact]
    public async Task Update_WhenCategoryIdIsNull()
    {
        // Arrange
        var oldCategory = new CategoryModel();
        var newCategory = new CategoryModel();

        // Act
        async Task Act()
        {
            await _data.Update(oldCategory, newCategory);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Act);
    }

    [Fact]
    public async Task Update_WhenCategoryIsNotInDatabase()
    {
        // Arrange
        var oldCategory = new CategoryModel { Id = Random.Shared.Next() };
        var newCategory = new CategoryModel();
        

        // Act
        async Task Act()
        {
            await _data.Update(oldCategory, newCategory);
        }

        // Assert
        await Assert.ThrowsAsync<DatabaseException>(Act);
    }

    [Fact]
    public async Task GetAllLeafCategories()
    {
        // Arrange
        const int count = 20;
        var rootCategories = new List<CategoryModel>();
        for (var i = 0; i < count; i++)
        {
            var category = CategoryGenerator.Generate();
            await _data.Create(category);
            rootCategories.Add(new CategoryModel { Id = category.Id, Name = category.Name });
        }

        var leafCategories = new List<CategoryModel>();
        for (var i = 0; i < Random.Shared.Next(count); i++)
        {
            var category = CategoryGenerator.Generate();
            category.ParentId = rootCategories[i].Id;
            await _data.Create(category);
            leafCategories.Add(new CategoryModel { Id = category.Id, Name = category.Name });
        }
        
        
        // Act
        var expected = leafCategories.Count;
        var result = await _data.GetLeafCategories();

        
        // Assert
        var actual = leafCategories.
            Select(x => x.Id)
            .Intersect(
                result.Select(x => x.Id)).Count();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        var createdCategories = new List<CategoryModel>();
        for (var i = 0; i < 20; i++)
        {
            var category = CategoryGenerator.Generate();
            await _data.Create(category);
            createdCategories.Add(new CategoryModel { Id = category.Id, Name = category.Name });
        }

        // Act
        var expected = createdCategories.Count;
        var result = await _data.GetAll();


        // Assert
        var actual = createdCategories.Select(x => x.Id)
            .Intersect(
                result.Select(x => x.Id)).Count();

        Assert.Equal(expected, actual);
    }
}