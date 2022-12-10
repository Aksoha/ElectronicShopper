namespace ElectronicShopper.Library.Settings;

/// <summary>
///     Represents Images section in appsettings.json.
/// </summary>
public class ImageStorageSettings
{
    /// <summary>
    ///     Base Url to image folder.
    /// </summary>
    public string BasePath { get; set; } = string.Empty;


    /// <summary>
    ///     Product image folder name.
    /// </summary>
    public string Products { get; set; } = string.Empty;


    /// <summary>
    ///     Size limit of images.
    /// </summary>
    public int SizeLimit { get; set; }

    /// <summary>
    ///     Maximum amount of files that can be inserted for one product.
    /// </summary>
    public int MaximumFileCount { get; set; }
}