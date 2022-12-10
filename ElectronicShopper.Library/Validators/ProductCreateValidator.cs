using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="ProductModel" /> can be used for inserting new product into
///     the database.
/// </summary>
public class ProductCreateValidator : AbstractValidator<ProductInsertModel>
{
    public ProductCreateValidator(IValidator<MemoryImageModel> imageValidator)
    {
        RuleFor(x => x)
            .Must(HaveConsistentTemplateAndProperty)
            .WithMessage(
                "product properties names (keys) should have corresponding name in template properties and vice-versa");
        RuleFor(x => x.CategoryId).NotNull();
        RuleForEach(x => x.Images).SetValidator(imageValidator);
        When(x => x.Images.Count > 0, () =>
        {
            RuleFor(x => x.Images)
                .Must(HaveUniqueNames).WithMessage("Images must have unique names.")
                .Must(HaveOnlyOnePrimaryImage).WithMessage("Images must have exactly one primary image.");
        });
        Transform(x => x.ProductName, s => s.Trim()).NotEmpty();
    }

    /// <summary>
    ///     Determines whether <see cref="ProductInsertModel.Template" /> and <see cref="ProductInsertModel.Properties" />
    ///     have consistent values.
    /// </summary>
    /// <param name="product">Model to validate.</param>
    /// <returns>
    ///     <see langword="true" /> when all names (keys) of <see cref="ProductModel.Properties">product properties</see>
    ///     have matching value in <see cref="ProductTemplateModel.Properties">template property</see> and vice-versa,
    ///     otherwise <see langword="false" />.
    /// </returns>
    private static bool HaveConsistentTemplateAndProperty(ProductInsertModel product)
    {
        return product.Template switch
        {
            null when product.Properties.Count == 0 => true,
            null when product.Properties.Count > 0 => false,
            _ => product.Template!.Properties.Count == product.Properties.Count &&
                 product.Template.Properties.All(templateProperty => product.Properties.Keys.Contains(templateProperty))
        };
    }


    /// <summary>
    ///     Determines whether all <see cref="ProductInsertModel.Images" /> have unique <see cref="MemoryImageModel.Name" />.
    /// </summary>
    /// <param name="images">Collection to validate.</param>
    /// <returns><see langword="true" />if model passes validation, otherwise false.</returns>
    private static bool HaveUniqueNames(IEnumerable<MemoryImageModel> images)
    {
        var imagesArray = images as MemoryImageModel[] ?? images.ToArray();
        return imagesArray.DistinctBy(x => x.Name).Count() == imagesArray.Length;
    }


    /// <summary>
    ///     Determines whether <see cref="ProductInsertModel" /> has exactly one
    ///     <see cref="ProductImageModel.IsPrimary">primary</see> image.
    /// </summary>
    /// <param name="images">Collection to validate.</param>
    /// <returns><see langword="true" />if model passes validation, otherwise false.</returns>
    private static bool HaveOnlyOnePrimaryImage(IEnumerable<MemoryImageModel> images)
    {
        return images.Count(x => x.IsPrimary) == 1;
    }
}