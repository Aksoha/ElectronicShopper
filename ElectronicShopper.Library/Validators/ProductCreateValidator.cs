using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class ProductCreateValidator : AbstractValidator<ProductInsertModel>
{
    public ProductCreateValidator(IValidator<MemoryImageModel> imageValidator)
    {
        RuleFor(x => x).Must(HaveConsistentTemplateAndProperty);
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

    private static bool HaveUniqueNames(IEnumerable<MemoryImageModel> images)
    {
        var imagesArray = images as MemoryImageModel[] ?? images.ToArray();
        return imagesArray.DistinctBy(x => x.Name).Count() == imagesArray.Length;
    }

    private static bool HaveOnlyOnePrimaryImage(IEnumerable<MemoryImageModel> images)
    {
        return images.Count(x => x.IsPrimary) == 1;
    }
}