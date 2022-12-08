using System.Data;
using System.Diagnostics;
using System.Reflection;
using AutoMapper;
using Dapper;
using ElectronicShopper.Library;
using ElectronicShopper.Library.Data;
using ElectronicShopper.Library.DependencyInjection;
using ElectronicShopper.Library.Settings;
using ElectronicShopper.Library.Validators;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ElectronicShopper.Tests.Integration;

[CollectionDefinition("database collection")]
public class DatabaseFactory : ICollectionFixture<DatabaseFactory>, IDisposable
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly IFileSystem _fileSystem = new FileSystem();
    private IConfiguration _config = default!;


    private IMapper _mapper = default!;

    public DatabaseFactory()
    {
        CreateConfig();
        ConfigureMapper();
        ConfigureSettings();
        ConfigureTestServices(_mapper);
        CreateTestDirectory();
    }

    public OrderData OrderData { get; private set; } = default!;
    public ProductData ProductData { get; private set; } = default!;
    public CategoryData CategoryData { get; private set; } = default!;
    private IOptionsSnapshot<ImageStorageSettings> ImageSettings { get; set; } = default!;
    private IOptionsSnapshot<ConnectionStringSettings> ConnectionSettings { get; set; } = default!;
    private ConnectionStringSettings ConnectionStringSettings => ConnectionSettings.Value;
    private ImageStorageSettings ImageStorageSettings => ImageSettings.Value;

    public void Dispose()
    {
        try
        {
            _fileSystem.DeleteDirectory($"{ImageStorageSettings.BasePath}/{ImageStorageSettings.Products}", true);
        }
        catch
        {
            // ignored, folder was not created during test
        }
    }


    /// <summary>
    ///     Resets database to it's initial state.
    /// </summary>
    /// <remarks>Removes all tables and reset Id counter for tables.</remarks>
    public async Task ResetDatabaseAndClearCache()
    {
        using IDbConnection dbConnection = new SqlConnection(ConnectionStringSettings.ElectronicShopperData);

        var dir = Directory.GetCurrentDirectory() + "\\ClearTables.sql";
        var query = await File.ReadAllTextAsync(dir);

        ResetCache();
        await dbConnection.ExecuteAsync(query);
    }

    /// <summary>
    ///     Clear all <see cref="IMemoryCache" /> from memory.
    /// </summary>
    /// <exception cref="UnreachableException">
    ///     Throw when <see cref="MemoryCache" /> does not contain expected fields.
    ///     This exception should not be thrown unless underlying structure of <see cref="MemoryCache" /> has changed,
    ///     which can be a case when changing NET version.
    /// </exception>
    /// <remarks>
    ///     Calling this method is with conjunction of <see cref="ResetDatabaseAndClearCache" /> required after each test to
    ///     ensure that there are no objects from previous test.
    ///     Not calling this method after a test could cause a scenario where stored in the database and cache are different
    ///     (even though Id's of object are the same).
    /// </remarks>
    public void ResetCache()
    {
        _cache.Clear();
    }

    private void CreateConfig()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.UnitTest.json")
            .AddUserSecrets(typeof(DatabaseFactory).Assembly, true)
            .Build();
    }

    private void ConfigureMapper()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new DatabaseMappingProfile()); });
        _mapper = mapperConfiguration.CreateMapper();
    }

    private void ConfigureTestServices(IMapper mapper)
    {
        var orderDetailValidator = new OrderDetailCreateValidator();
        var orderValidator = new OrderCreateValidator(orderDetailValidator);
        var categoryValidator = new CategoryCreateValidator();
        var imageValidator = new ProductImageCreateValidator();
        var productValidator = new ProductCreateValidator(imageValidator);
        var templateValidator = new ProductTemplateCreateValidator();
        var productDataLogger = Mock.Of<ILogger<ProductData>>();
        var categoryDataLogger = Mock.Of<ILogger<CategoryData>>();


        var sql = new SqlDataAccess();
        OrderData = new OrderData(sql, mapper, ConnectionSettings, orderValidator);
        CategoryData = new CategoryData(sql, mapper, ConnectionSettings, categoryValidator, _cache, categoryDataLogger);
        ProductData = new ProductData(sql, mapper, CategoryData, ConnectionSettings, ImageSettings, _fileSystem,
            productValidator, imageValidator, templateValidator, productDataLogger, _cache);
    }

    private void ConfigureSettings()
    {
        var connectionStringSettings = new ConnectionStringSettings
        {
            DefaultConnection = TrustCertificate(_config.GetConnectionString("DefaultConnection") ??
                                                 throw new InvalidOperationException()),
            ElectronicShopperData = TrustCertificate(_config.GetConnectionString("ElectronicShopperData") ??
                                                     throw new InvalidOperationException())
        };

        var imageStorageSettings = new ImageStorageSettings
        {
            BasePath = _config["Images:BasePath"] ?? throw new InvalidOperationException(),
            Products = _config["Images:Products"] ?? throw new InvalidOperationException()
        };

        var connectionMock = new Mock<IOptionsSnapshot<ConnectionStringSettings>>();
        connectionMock.Setup(x => x.Value).Returns(connectionStringSettings);
        ConnectionSettings = connectionMock.Object;

        var imageMock = new Mock<IOptionsSnapshot<ImageStorageSettings>>();
        imageMock.Setup(x => x.Value).Returns(imageStorageSettings);
        ImageSettings = imageMock.Object;
    }


    private static string TrustCertificate(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            TrustServerCertificate = true
        };
        return builder.ConnectionString;
    }

    private void CreateTestDirectory()
    {
        // clear in case dispose was not called
        if (_fileSystem.Exists(ImageStorageSettings.BasePath))
            _fileSystem.DeleteDirectory(ImageStorageSettings.BasePath, true);

        _fileSystem.CreateDirectory($"{ImageStorageSettings.BasePath}/{ImageStorageSettings.Products}");
    }
}