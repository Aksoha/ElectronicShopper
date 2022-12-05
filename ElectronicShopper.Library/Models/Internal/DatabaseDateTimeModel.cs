namespace ElectronicShopper.Library.Models.Internal;

internal class DatabaseDateTimeModel : IDbEntity
{
    /// <summary>
    /// Id of database object
    /// </summary>
    public int Id { get; set; }
    public DateTime? DateTime { get; set; }
}