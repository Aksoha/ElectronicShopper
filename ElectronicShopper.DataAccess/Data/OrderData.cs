using AutoMapper;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Settings;
using Microsoft.Extensions.Options;

namespace ElectronicShopper.DataAccess.Data;

public class OrderData : IOrderData
{
    private readonly ISqlDataAccess _sql;
    private readonly IMapper _mapper;
    private readonly string _connectionString;

    public OrderData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings)
    {
        _sql = sql;
        _mapper = mapper;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task AddOrder(OrderModel order)
    {
        _sql.StartTransaction(_connectionString);

        var id = await _sql.SaveData<dynamic, int>("spOrder_Insert", new { order.UserId });

        foreach (var item in order.PurchasedProducts)
        {
            await _sql.SaveData("spOrderDetails_Insert",
                new { item.ProductId, OrderId = id, item.Quantity, item.PricePerItem });
        }

        _sql.CommitTransaction();

        order.Id = id;
    }
}