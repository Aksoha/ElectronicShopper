namespace ElectronicShopper.Library.Models;

public class ProductImageModel : IDbEntity
{
    public int? Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
    public string Extension => System.IO.Path.GetExtension(Path);
    public bool IsPrimary { get; set; }
}