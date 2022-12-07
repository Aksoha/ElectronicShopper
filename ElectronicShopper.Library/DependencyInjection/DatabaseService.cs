using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicShopper.Library.DependencyInjection;

public static class DatabaseServiceExtensions
{
    public static IServiceCollection AddDatabaseService(this IServiceCollection services)
    {
        services.AddScoped<IValidator<OrderModel>, OrderCreateValidator>();
        services.AddScoped<IValidator<OrderDetailModel>, OrderDetailCreateValidator>();
        services.AddScoped<IValidator<ProductInsertModel>, ProductCreateValidator>();
        services.AddScoped<IValidator<MemoryImageModel>, ProductImageCreateValidator>();
        services.AddScoped<IValidator<CategoryCreateModel>, CategoryCreateValidator>();
        services.AddScoped<IValidator<ProductTemplateModel>, ProductTemplateCreateValidator>();
        services.AddScoped<IFileSystem, FileSystem>();

        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ICategoryData, CategoryData>();
        services.AddTransient<IOrderData, OrderData>();
        services.AddTransient<IProductData, ProductData>();

        services.AddMemoryCache();
        
        return services;
    }
}