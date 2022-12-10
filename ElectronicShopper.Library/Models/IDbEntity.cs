namespace ElectronicShopper.Library.Models;

/// <summary>
///     Represents an entity that resides in the database.
/// </summary>
public interface IDbEntity
{
    /// <summary>
    ///     Id that represent primary key in database table.
    /// </summary>
    int? Id { get; set; }
}