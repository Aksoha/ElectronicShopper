using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public interface IInventoryData
{
    Task<List<InventoryModel>> GetAll();
    Task<InventoryModel?> GetInventory(ProductModel product);
    Task UpdateInventory(InventoryModel inventory);
}