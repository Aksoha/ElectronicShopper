namespace ElectronicShopper.Library.Settings;

/// <summary>
///     Represents ConnectionString section in appsettings.json.
/// </summary>
public class ConnectionStringSettings
{
    /// <summary>
    ///     Default connection used by the entity framework for handling authentication/authorization.
    /// </summary>
    public string DefaultConnection { get; set; } = string.Empty;

    /// <summary>
    ///     Connection to the database containing information about products, categories and inventory.
    /// </summary>
    public string ElectronicShopperData { get; set; } = string.Empty;
}