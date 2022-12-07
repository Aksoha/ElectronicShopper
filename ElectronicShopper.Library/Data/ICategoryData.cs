namespace ElectronicShopper.Library.Data;

/// <summary>
///     Provides access to categories stored in the database.
/// </summary>
public interface ICategoryData
{
    /// <summary>
    ///     Adds category to the database.
    /// </summary>
    /// <param name="category">Category to add.</param>
    /// <exception cref="DatabaseException">
    ///     Thrown when <see cref="CategoryCreateModel.ParentId" /> is not <see langword="null" />
    ///     but parent is not present in the database.
    /// </exception>
    /// <exception cref="FluentValidation.ValidationException">
    ///     Thrown when <see cref="CategoryCreateModel.Name" /> is empty or whitespace.
    /// </exception>
    Task Create(CategoryCreateModel category);


    /// <summary>
    ///     Retrieve specific category.
    /// </summary>
    /// <param name="id">Id of category.</param>
    Task<CategoryModel?> Get(int id);

    /// <summary>
    ///     Retrieve all categories from the database.
    /// </summary>
    Task<IEnumerable<CategoryModel>> GetAll();


    /// <summary>
    ///     Retrieve all categories that do not have a child category.
    /// </summary>
    Task<IEnumerable<CategoryModel>> GetRootCategories();


    /// <summary>
    ///     Retrieve all categories that do not have a parent category.
    /// </summary>
    Task<IEnumerable<CategoryModel>> GetLeafCategories();

    /// <summary>
    ///     Updates category.
    /// </summary>
    /// <param name="oldCategory">The old category.</param>
    /// <param name="newCategory">The new category.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <see cref="CategoryModel.Id" /> of
    ///     <paramref name="oldCategory" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="DatabaseException">
    ///     Thrown when <see cref="CategoryModel.Id" /> of
    ///     <paramref name="newCategory" /> parent is not <see langword="null" />
    ///     but parent is not present in the database.
    /// </exception>
    Task Update(CategoryModel oldCategory, CategoryModel newCategory);
}