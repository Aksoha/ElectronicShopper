using Microsoft.Extensions.DependencyInjection;

namespace ElectronicShopper.DataAccess.DependencyInjection;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services)
    {
        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ICategoryData, CategoryData>();
        services.AddTransient<IInventoryData, InventoryData>();
        services.AddTransient<IProductData, ProductData>();
        services.AddTransient<IUserData, UserData>();

        return services;
    }
}