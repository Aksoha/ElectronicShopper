using AutoMapper;
using ElectronicShopper.DataAccess.Models.Internal;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public class InventoryData : IInventoryData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private const string ConnectionStringName = "ElectronicShopperData";

    
    public InventoryData(ISqlDataAccess sql, IMapper mapper)
    {
        _sql = sql;
        _mapper = mapper;
    }

    public async Task<List<InventoryModel>> GetAll()
    {
        _sql.StartTransaction(ConnectionStringName);
        var result = await _sql.LoadData<InventoryDbModel, dynamic>("spInventory_GetAll", null!);
        _sql.CommitTransaction();

        var output = _mapper.Map < List<InventoryModel>>(result);
        return output;
    }

    public async Task<InventoryModel?> GetInventory(ProductModel product)
    {
        _sql.StartTransaction(ConnectionStringName);
        var result = await _sql.LoadData<InventoryDbModel, dynamic>("spInventory_GetByProductId", new { ProductId = product.Id });
        _sql.CommitTransaction();

        var output = _mapper.Map<InventoryModel>(result.FirstOrDefault());
        return output;
    }

    public async Task UpdateInventory(InventoryModel inventory)
    {
        _sql.StartTransaction(ConnectionStringName);
        await _sql.SaveData("spInventory_UpdateQuantity", new { inventory.Id, inventory.Quantity });
        _sql.CommitTransaction();
    }
}