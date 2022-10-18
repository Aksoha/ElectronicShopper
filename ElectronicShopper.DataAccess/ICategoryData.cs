using ElectronicShopper.Library.Models;

namespace ElectronicShopper.DataAccess;

public interface ICategoryData
{
    Task<List<CategoryModel>> GetCategories();
    Task<CategoryModel?> GetById(int id);
    Task CreateCategory(CategoryModel category);
    Task RebaseCategory(CategoryModel category, CategoryModel newAncestor);
}