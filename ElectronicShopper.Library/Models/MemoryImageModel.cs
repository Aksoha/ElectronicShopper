namespace ElectronicShopper.Library.Models;

/// <summary>
/// Represents an image which is stored in memory
/// </summary>
public class MemoryImageModel : IDbEntity
{
    public int? Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            _name = Path.GetFileNameWithoutExtension(value);
            Extension = Path.GetExtension(value);
        }
    }
    public string Extension { get; private set; } = string.Empty;
    public MemoryStream Stream { get; set; } = new();
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Image resource url
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

    private string _name = string.Empty;
}