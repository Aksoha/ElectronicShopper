using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicShopper.Library.DependencyInjection;

public static class DatabaseServiceExtensions
{
    /// <summary>
    ///     Adds services that are required in order to access data.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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