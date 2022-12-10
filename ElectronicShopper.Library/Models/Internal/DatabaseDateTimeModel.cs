namespace ElectronicShopper.Library.Models.Internal;

/// <summary>
///     A date time used for mapping datetime2 column from the database.
/// </summary>
internal class DatabaseDateTimeModel : IDbEntity
{
    public DateTime? DateTime { get; set; }

    /// <summary>
    ///     Id of database object.
    /// </summary>
    public int Id { get; set; }
}