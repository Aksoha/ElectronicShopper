using ElectronicShopper.DataAccess.StoredProcedures.Order;
using ElectronicShopper.Library.Models;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace ElectronicShopper.DataAccess.Data;

public class OrderData : IOrderData
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;
    private readonly IValidator<OrderModel> _validator;
    private readonly ISqlDataAccess _sql;

    public OrderData(ISqlDataAccess sql, IMapper mapper, IOptionsSnapshot<ConnectionStringSettings> settings,
        IValidator<OrderModel> validator)
    {
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(sql);
        
        _sql = sql;
        _mapper = mapper;
        _validator = validator;
        _connectionString = settings.Value.ElectronicShopperData;
    }

    public async Task Create(OrderModel order)
    {
        await _validator.ValidateAndThrowAsync(order);

        var spOrder = _mapper.Map<OrderInsertStoredProcedure>(order);


        DatabaseDateTimeModel result;
        try
        {
            _sql.StartTransaction(_connectionString);
            result = await _sql.SaveData<OrderInsertStoredProcedure, DatabaseDateTimeModel>(spOrder);

            foreach (var orderDetail in order.PurchasedProducts)
            {
                var spOrderDetail = _mapper.Map<OrderDetailsInsertStoredProcedure>(orderDetail);
                spOrderDetail.OrderId = result.Id;
                var detailId = await _sql.SaveData<OrderDetailsInsertStoredProcedure, int>(spOrderDetail);
                orderDetail.Id = detailId;
            }

            _sql.CommitTransaction();
        }
        catch (SqlException e)
        {
            _sql.RollbackTransaction();
            if (e.Message == "Insufficient quantity")
                throw new DatabaseException("Insufficient quantity", e);
            throw;
        }
        
        order.Id = result.Id;
        order.PurchaseTime = (DateTime)result.DateTime!;
    }
}