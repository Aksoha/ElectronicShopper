using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using ElectronicShopper.Library.Models.Internal;
using ElectronicShopper.Library.StoredProcedures.Category;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using IDbEntity = ElectronicShopper.Library.Models.IDbEntity;

namespace ElectronicShopper.Library.Data;

public class CategoryData : ICategoryData
{
    /// <summary>
    ///     Key value used to cache <see cref="CategoryModel" />.
    /// </summary>
    private const string CacheName = "CategoryData";

    private readonly IMemoryCache _cache;

    /// <summary>
    ///     Time after which cache will be removed.
    /// </summary>
    private readonly TimeSpan _cacheTime = TimeSpan.FromDays(1);

    private readonly IValidator<CategoryCreateModel> _categoryValidator;
    private readonly string _connectionString;
    private readonly ILogger<CategoryData> _logger;
    private readonly IMapper _mapper;
    private readonly ISqlDataAccess _sql;


    public CategoryData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings,
        IValidator<CategoryCreateModel> categoryValidator, IMemoryCache cache, ILogger<CategoryData> logger)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sql);
        ArgumentNullException.ThrowIfNull(categoryValidator);

        _sql = sql;
        _mapper = mapper;
        _categoryValidator = categoryValidator;
        _cache = cache;
        _logger = logger;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task Create(CategoryCreateModel category)
    {
        // validate if model passes all criteria to be inserted
        if (category.ParentId is not null) await ThrowIfCategoryDoesNotExist(category.ParentId);
        await _categoryValidator.ValidateAndThrowAsync(category);


        // insert into database
        var sp = _mapper.Map<CategoryInsertStoredProcedure>(category);
        try
        {
            _sql.StartTransaction(_connectionString);
            var id = await _sql.SaveData<CategoryInsertStoredProcedure, int>(sp);
            _sql.CommitTransaction();
            category.Id = id;
            _logger.LogInformation("Created new row in category table with Id {Id} and name {Name}", category.Id,
                category.Name);
            var c = _mapper.Map<CategoryModel>(category);
            await SetParent(c);
            AddCategoryToCache(c);
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task<CategoryModel?> Get(int id)
    {
        // try to get category from cache
        var cachedCategory = GetCategoryFromCache(id);
        if (cachedCategory is not null)
        {
            _logger.LogDebug("Fetched category with Id {Id} and Name {Name} from cache", id,
                cachedCategory.Name);
            return cachedCategory;
        }

        // try to get category from database
        var sp = _mapper.Map<CategoryGetByIdStoredProcedure>(id);
        CategoryDbModel? result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result = (await _sql.LoadData<CategoryGetByIdStoredProcedure, CategoryDbModel>(sp)).FirstOrDefault();
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        // category not found in cache and database
        if (result is null)
        {
            _logger.LogDebug("Category with Id {Id} was not found", id);
            return null;
        }

        // convert database model and cache category
        var output = MapToSingle(result);
        _logger.LogDebug("Fetched category with Id {Id} and Name {Name} from database", id,
            output.Name);
        await SetParent(output);
        AddCategoryToCache(output);
        return output;
    }

    public async Task<IEnumerable<CategoryModel>> GetAll()
    {
        // try to get categories from cache
        var cachedCategories = GetCategoriesFromCache();
        if (cachedCategories is not null)
        {
            _logger.LogDebug("Fetched all categories from cache");
            return cachedCategories;
        }


        // try to get category from database
        IEnumerable<CategoryDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result =
                await _sql.LoadData<CategoryGetAllStoredProcedure, CategoryDbModel>(
                    new CategoryGetAllStoredProcedure());
            _sql.CommitTransaction();
            _logger.LogDebug("Fetched all categories from database");
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }


        // convert database models and cache categories
        var output = MapToMany(result);
        foreach (var item in output)
            await SetParent(item);

        AddCategoryToCache(output);
        return output;
    }

    public async Task<IEnumerable<CategoryModel>> GetRootCategories()
    {
        var categories = await GetAll();
        return categories.Where(x => x.Parent is null);
    }


    public async Task<IEnumerable<CategoryModel>> GetLeafCategories()
    {
        var categories = (await GetAll()).ToList();
        HashSet<CategoryModel> remainingNodes = new(categories);
        foreach (var parent in
                 from category in categories
                 select category.Parent?.Id
                 into percentId
                 where remainingNodes.Any(x => x.Id == percentId)
                 select remainingNodes.Single(x => x.Id == percentId))
            remainingNodes.Remove(parent);

        return remainingNodes;
    }

    public async Task Update(CategoryModel oldCategory, CategoryModel newCategory)
    {
        // validate if model passes all criteria to be inserted
        await ThrowIfCategoryDoesNotExist(oldCategory);
        if (newCategory.Parent is not null)
            await ThrowIfCategoryDoesNotExist(newCategory.Parent);


        // update category
        var sp = new CategoryUpdateStoredProcedure
        {
            Id = (int)oldCategory.Id!,
            ParentId = (int)newCategory.Id!,
            CategoryName = newCategory.Name
        };

        try
        {
            _sql.StartTransaction(_connectionString);
            await _sql.SaveData(sp);
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        oldCategory.Id = newCategory.Id;
        oldCategory.Name = newCategory.Name;
        oldCategory.Parent = newCategory.Parent;
    }

    /// <summary>
    ///     Sets all nested categories inside <paramref name="category" />.
    /// </summary>
    /// <param name="category">Category which parents are to be set.</param>
    private async Task SetParent(CategoryModel category)
    {
        // get parent of category from the database
        var sp = _mapper.Map<CategoryGetAncestorStoredProcedure>(category);
        _sql.StartTransaction(_connectionString);
        var result = (await _sql.LoadData<CategoryGetAncestorStoredProcedure, CategoryDbModel>(sp)).FirstOrDefault();
        _sql.CommitTransaction();

        // recursively set parent of category
        category.Parent = MapToSingle(result);
        if (category.Parent is not null) await SetParent(category.Parent);
    }

    /// <summary>
    ///     Validates that object is in the database.
    /// </summary>
    /// <param name="category">Object to validate.</param>
    /// <exception cref="NullReferenceException">Thrown when <see cref="IDbEntity.Id" /> is <see langword="null" />.</exception>
    /// <exception cref="DatabaseException">Thrown when category is not in the database.</exception>
    private async Task ThrowIfCategoryDoesNotExist(IDbEntity category)
    {
        await ThrowIfCategoryDoesNotExist(category.Id);
    }

    /// <summary>
    ///     Validates that object is in the database.
    /// </summary>
    /// <param name="id">Id of object to validate.</param>
    /// <exception cref="NullReferenceException">Thrown when <paramref name="id" /> is <see langword="null" />.</exception>
    /// <exception cref="DatabaseException">Thrown when category is not in the database.</exception>
    private async Task ThrowIfCategoryDoesNotExist([NotNull] int? id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var dbCategory = await Get((int)id);
        if (dbCategory is null)
            throw new DatabaseException("Category is not present in the database");
    }

    /// <summary>
    ///     Maps database models to collection of regular models.
    /// </summary>
    /// <param name="dbItems">Items to map.</param>
    private List<CategoryModel> MapToMany(IEnumerable<CategoryDbModel> dbItems)
    {
        return _mapper.Map<List<CategoryModel>>(dbItems);
    }

    /// <summary>
    ///     Maps database model to regular models.
    /// </summary>
    /// <param name="dbItem">Item to map.</param>
    [return: NotNullIfNotNull("dbItem")]
    private CategoryModel? MapToSingle(CategoryDbModel? dbItem)
    {
        return _mapper.Map<CategoryModel>(dbItem);
    }


    /// <summary>
    ///     Creates new collection of items that is stored in the memory.
    /// </summary>
    /// <returns>Newly created collection.</returns>
    private ConcurrentBag<CategoryModel> CreateCacheIfItNotExist()
    {
        var cachedCategories = GetCategoriesFromCache();
        if (cachedCategories is not null) return cachedCategories;

        cachedCategories = new ConcurrentBag<CategoryModel>();
        _cache.Set(CacheName, cachedCategories, _cacheTime);
        _logger.LogDebug("Created new category collection with hash {Hash} in cache", cachedCategories.GetHashCode());

        return cachedCategories;
    }

    /// <summary>
    ///     Adds <see cref="CategoryModel" /> to cache.
    /// </summary>
    /// <param name="category">Category to be added.</param>
    private void AddCategoryToCache(CategoryModel category)
    {
        var cachedCategories = CreateCacheIfItNotExist();
        if (cachedCategories.Contains(category)) return;

        cachedCategories.Add(category);
        _logger.LogDebug("Added category with Id {Id} to cache", category.Id);
    }


    /// <summary>
    ///     Adds <see cref="CategoryModel" /> to cache.
    /// </summary>
    /// <param name="categories">Categories to be added.</param>
    private void AddCategoryToCache(IEnumerable<CategoryModel> categories)
    {
        var cachedCategories = CreateCacheIfItNotExist();
        foreach (var category in categories)
        {
            if (cachedCategories.Contains(category)) continue;
            cachedCategories.Add(category);
        }
    }

    /// <summary>
    ///     Retrieves <see cref="CategoryModel" /> from cache.
    /// </summary>
    /// <param name="id">Id of category to retrieve.</param>
    /// <returns><see cref="CategoryModel" /> if object was found, otherwise <see langoword="null" />.</returns>
    private CategoryModel? GetCategoryFromCache(int id)
    {
        var cachedCategories = GetCategoriesFromCache();
        return cachedCategories?.SingleOrDefault(x => x.Id == id);
    }


    /// <summary>
    ///     Retrieves collection of <see cref="CategoryModel" /> from cache.
    /// </summary>
    /// <returns>Collection if cache was set or not expired otherwise <see langoword="null" />.</returns>
    private ConcurrentBag<CategoryModel>? GetCategoriesFromCache()
    {
        return _cache.Get<ConcurrentBag<CategoryModel>>(CacheName);
    }
}