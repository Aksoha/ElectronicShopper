using ElectronicShopper.DataAccess.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicShopper.DataAccess.DependencyInjection;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services)
    {
        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ICategoryData, CategoryData>();
        services.AddTransient<IInventoryData, InventoryData>();
        services.AddTransient<IOrderData, OrderData>();
        services.AddTransient<IProductData, ProductData>();

        return services;
    }
}