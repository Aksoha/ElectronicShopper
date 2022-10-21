using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess.Data;

public class CategoryData : ICategoryData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private const string ConnectionStringName = "ElectronicShopperData";

    public CategoryData(ISqlDataAccess sql, IMapper mapper)
    {
        _sql = sql;
        _mapper = mapper;
    }

    public async Task<List<CategoryModel>> GetLeafCategories()
    {
        _sql.StartTransaction(ConnectionStringName);
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
        _sql.StartTransaction(ConnectionStringName);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetRoot", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map<List<CategoryModel>>(result);
        return output;
    }
    
    public async Task<CategoryModel?> GetById(int id)
    {
        _sql.StartTransaction(ConnectionStringName);
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
        _sql.StartTransaction(ConnectionStringName);
        var id = await _sql.SaveData<dynamic, int>("spCategory_Insert",
            new { ParentId = category.Ancestors?.Last().Id, CategoryName = category.Name });
        _sql.CommitTransaction();
        category.Id = id;
    }

    public async Task RebaseCategory(CategoryModel category, CategoryModel newAncestor)
    {
        _sql.StartTransaction(ConnectionStringName);
        await _sql.SaveData("spCategory_Update",
            new { category.Id, ParentId = newAncestor.Id, CategoryName = category.Name });
        _sql.CommitTransaction();
        await GetAncestors(category);
    }

    private async Task GetAncestors(CategoryModel category)
    {
        _sql.StartTransaction(ConnectionStringName);
        var result = await _sql.LoadData<CategoryDbModel, dynamic>("dbo.spCategory_GetAncestors", new { category.Id });
        _sql.CommitTransaction();

        category.Ancestors = _mapper.Map<List<CategoryModel>>(result);

        foreach (var item in category.Ancestors)
        {
            await GetAncestors(item);
        }
    }
}