using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Settings;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.DataAccess.Data;

public class CategoryData : ICategoryData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private readonly string _connectionString;

    public CategoryData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings)
    {
        _sql = sql;
        _mapper = mapper;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task<List<CategoryModel>> GetLeafCategories()
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetAllLeafs", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map<List<CategoryModel>>(result);

        foreach (var category in output)
        {
            await GetAncestors(category);
        }

        return output;
    }

    public async Task<List<CategoryModel>> GetRootCategories()
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetRoot", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map<List<CategoryModel>>(result);
        return output;
    }
    
    public async Task<CategoryModel?> GetById(int id)
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetById", new {Id = id});
        _sql.CommitTransaction();

        var output = _mapper.Map<CategoryModel>(result.SingleOrDefault());

        if (output is not null)
        {
            await GetAncestors(output);
        }

        return output;
    }

    public async Task CreateCategory(CategoryModel category)
    {
        _sql.StartTransaction(_connectionString);
        var id = await _sql.SaveData<dynamic, int>("spCategory_Insert",
            new { ParentId = category.Ancestors?.Last().Id, CategoryName = category.Name });
        _sql.CommitTransaction();
        category.Id = id;
    }

    public async Task RebaseCategory(CategoryModel category, CategoryModel newAncestor)
    {
        _sql.StartTransaction(_connectionString);
        await _sql.SaveData("spCategory_Update",
            new { category.Id, ParentId = newAncestor.Id, CategoryName = category.Name });
        _sql.CommitTransaction();
        await GetAncestors(category);
    }

    private async Task GetAncestors(CategoryModel category)
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetAncestors", new { category.Id });
        _sql.CommitTransaction();

        category.Ancestors = _mapper.Map<List<CategoryModel>>(result);

        foreach (var item in category.Ancestors)
        {
            await GetAncestors(item);
        }
    }
}