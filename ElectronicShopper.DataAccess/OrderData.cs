using AutoMapper;
using ElectronicShopper.DataAccess.Models;
using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public class OrderData : IOrderData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private const string ConnectionStringName = "ElectronicShopperData";
    
    public OrderData(ISqlDataAccess sql, IMapper mapper)
    {
        _sql = sql;
        _mapper = mapper;
    }

    public async Task AddOrder(OrderModel order)
    {
        _sql.StartTransaction(ConnectionStringName);

        var id = await _sql.SaveData<dynamic, int>("spOrder_Insert", new {order.UserId});
        
        foreach (var item in order.PurchasedProducts)
        {
            await _sql.SaveData("spOrderDetails_Insert",
                new {item.ProductId, OrderId = id, item.Quantity, item.PricePerItem });
        }
        
        _sql.CommitTransaction();
        
        order.Id = id;
    }
}