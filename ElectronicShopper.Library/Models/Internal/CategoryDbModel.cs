namespace ElectronicShopper.Library.Models.Internal;


/// <summary>
///     This class represents database Category table.
/// </summary>
/// <seealso cref="CategoryModel"/>
internal class CategoryDbModel : IDbEntity
{
    public int Id { get; set; }
    
    /// <summary>
    ///     Id of parent category.
    /// </summary>
    public int? ParentId { get; set; }
    

    /// <inheritdoc cref="CategoryModel.Name"/>
    public string CategoryName { get; set; } = string.Empty;
}