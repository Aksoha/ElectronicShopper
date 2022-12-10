namespace ElectronicShopper.Library.Models;

/// <summary>
///     Represents an image which is stored in memory.
/// </summary>
public class MemoryImageModel : IDbEntity
{
    private string _name = string.Empty;

    /// <inheritdoc cref="ProductImageModel.Name" />
    public string Name
    {
        get => _name;
        set
        {
            _name = Path.GetFileNameWithoutExtension(value);
            Extension = Path.GetExtension(value);
        }
    }

    /// <inheritdoc cref="ProductImageModel.Extension" />
    public string Extension { get; private set; } = string.Empty;


    /// <summary>
    ///     Stream containing bytes of image.
    /// </summary>
    public MemoryStream Stream { get; set; } = new();


    /// <inheritdoc cref="ProductImageModel.IsPrimary" />
    public bool IsPrimary { get; set; }

    /// <summary>
    ///     Base64 representation of the <see cref="Stream" />.
    /// </summary>
    public string Url
    {
        get
        {
            var byteArray = Stream.ToArray();
            var b64String = Convert.ToBase64String(byteArray);
            return "data:image/png;base64," + b64String;
        }
    }

    public int? Id { get; set; }
}