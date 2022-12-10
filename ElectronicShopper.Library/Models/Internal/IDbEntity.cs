namespace ElectronicShopper.Library.Models.Internal;


/// <inheritdoc cref="Models.IDbEntity"/>
internal interface IDbEntity
{
    /// <inheritdoc cref="Models.IDbEntity.Id"/>
    public int Id { get; set; }
}