using ElectronicShopper.DataAccess.Data;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Models;
using ElectronicShopper.Library.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicShopper.DataAccess.DependencyInjection;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services)
    {
        services.AddScoped<IValidator<OrderModel>, OrderCreateValidator>();
        services.AddScoped<IValidator<OrderDetailModel>, OrderDetailCreateValidator>();
        services.AddScoped<IValidator<ProductInsertModel>, ProductCreateValidator>();
        services.AddScoped<IValidator<MemoryImageModel>, ProductImageCreateValidator>();
        services.AddScoped<IFileSystem, FileSystem>();

        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ICategoryData, CategoryData>();
        services.AddTransient<IOrderData, OrderData>();
        services.AddTransient<IProductData, ProductData>();

        return services;
    }
}