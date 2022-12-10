namespace ElectronicShopper.Library.Models;

/// <summary>
///     A class containing all the information's about the product.
/// </summary>
public class ProductModel : IDbEntity
{
    /// <summary>
    ///     Category to which product belong.
    /// </summary>
    public CategoryModel Category { get; set; } = new();

    /// <summary>
    ///     Inventory of the product.
    /// </summary>
    public InventoryModel Inventory { get; set; } = new();

    /// <summary>
    ///     Name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    ///     Images associated with the product.
    /// </summary>
    public List<ProductImageModel> Images { get; set; } = new();


    /// <summary>
    ///     A key value pair where key is the name of the property and value is a list of associated with the key.
    ///     <example>
    ///         A product containing property: "Ports" where "Ports" are "1x USB 3.0" and "2x USB 3.2"
    ///         should be stored as fallowing:
    ///         <code>
    ///             Properties.Add("Ports", new() {"1x USB 3.0", "2x USB 3.2"});
    ///         </code>
    ///     </example>
    /// </summary>
    public Dictionary<string, List<string>> Properties { get; set; } = new();


    /// <summary>
    ///     Indicates whether product is available for purchase.
    /// </summary>
    public bool Available => Inventory.Quantity > 0;


    /// <summary>
    ///     Indicated whether product is discontinued (true) or temporarily unavailable (false).
    /// </summary>
    public bool Discontinued { get; set; }

    public int? Id { get; set; }
}