namespace ElectronicShopper.Library.Models;

/// <summary>
///     Represents an image of the product.
/// </summary>
public class ProductImageModel : IDbEntity
{
    /// <summary>
    ///     Path to image.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    ///     Name of the file without extension.
    /// </summary>
    public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);

    /// <summary>
    ///     Extension of the image file.
    /// </summary>
    public string Extension => System.IO.Path.GetExtension(Path);

    /// <summary>
    ///     Indicates whether image is primary. Primary image is used in places where there is only one place for a product
    ///     image.
    /// </summary>
    public bool IsPrimary { get; set; }

    public int? Id { get; set; }
}