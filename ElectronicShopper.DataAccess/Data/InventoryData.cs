using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Settings;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.DataAccess.Data;

public class InventoryData : IInventoryData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private readonly string _connectionString;

    
    public InventoryData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings)
    {
        _sql = sql;
        _mapper = mapper;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task<List<InventoryModel>> GetAll()
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<InventoryDbModel, dynamic>("spInventory_GetAll", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map < List<InventoryModel>>(result);
        return output;
    }

    public async Task<InventoryModel?> GetInventory(ProductModel product)
    {
        _sql.StartTransaction(_connectionString);
        var result = await _sql.LoadData<InventoryDbModel, dynamic>("spInventory_GetByProductId", new { ProductId = product.Id });
        _sql.CommitTransaction();

        var output = _mapper.Map<InventoryModel>(result.FirstOrDefault());
        return output;
    }

    public async Task UpdateInventory(InventoryModel inventory)
    {
        _sql.StartTransaction(_connectionString);
        await _sql.SaveData("spInventory_UpdateQuantity", new { inventory.Id, inventory.Quantity });
        _sql.CommitTransaction();
    }
}