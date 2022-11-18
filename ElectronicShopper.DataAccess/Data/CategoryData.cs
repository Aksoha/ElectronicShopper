using System.Diagnostics.CodeAnalysis;
using ElectronicShopper.DataAccess.StoredProcedures.Category;
using ElectronicShopper.Library.Models;
using Microsoft.Data.SqlClient;
using IDbEntity = ElectronicShopper.Library.Models.IDbEntity;

namespace ElectronicShopper.DataAccess.Data;

public class CategoryData : ICategoryData
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;
    private readonly ISqlDataAccess _sql;

    public CategoryData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sql);

        _sql = sql;
        _mapper = mapper;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task Create(CategoryCreateModel category)
    {
        if (category.ParentId is not null) await ThrowIfCategoryDoesNotExist(category.ParentId);

        var sp = _mapper.Map<CategoryInsertStoredProcedure>(category);

        try
        {
            _sql.StartTransaction(_connectionString);
            var id = await _sql.SaveData<CategoryInsertStoredProcedure, int>(sp);
            _sql.CommitTransaction();
            category.Id = id;
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }
    }

    public async Task<CategoryModel?> Get(int id)
    {
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

        var output = MapToSingle(result);
        if (output is null)
            return null;
        await SetParent(output);
        return output;
    }

    public async Task<List<CategoryModel>> GetAll()
    {
        IEnumerable<CategoryDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result =
                await _sql.LoadData<CategoryGetAllStoredProcedure, CategoryDbModel>(
                    new CategoryGetAllStoredProcedure());
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        var output = MapToMany(result);
        return output;
    }

    public async Task<List<CategoryModel>> GetRootCategories()
    {
        IEnumerable<CategoryDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result = await _sql.LoadData<CategoryGetRootStoredProcedure, CategoryDbModel>(
                new CategoryGetRootStoredProcedure());
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        var output = MapToMany(result);
        return output;
    }

    public async Task<List<CategoryModel>> GetLeafCategories()
    {
        IEnumerable<CategoryDbModel> result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result = await _sql.LoadData<CategoryGetAllLeafsStoredProcedure, CategoryDbModel>(
                new CategoryGetAllLeafsStoredProcedure());
            _sql.CommitTransaction();
        }
        catch (SqlException)
        {
            _sql.RollbackTransaction();
            throw;
        }

        var output = MapToMany(result);
        foreach (var category in output) await SetParent(category);

        return output;
    }

    public async Task Update(CategoryModel oldCategory, CategoryModel newCategory)
    {
        await ThrowIfCategoryDoesNotExist(oldCategory);
        if (newCategory.Parent is not null)
            await ThrowIfCategoryDoesNotExist(newCategory.Parent);

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
        var sp = _mapper.Map<CategoryGetAncestorStoredProcedure>(category);
        _sql.StartTransaction(_connectionString);
        var result = (await _sql.LoadData<CategoryGetAncestorStoredProcedure, CategoryDbModel>(sp)).FirstOrDefault();
        _sql.CommitTransaction();

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
    private CategoryModel? MapToSingle(CategoryDbModel? dbItem)
    {
        return _mapper.Map<CategoryModel>(dbItem);
    }
}