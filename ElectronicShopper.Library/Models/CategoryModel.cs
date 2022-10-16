namespace ElectronicShopper.Library.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<CategoryModel>? Ancestors { get; set; }
}