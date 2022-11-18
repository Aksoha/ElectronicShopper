using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class ProductCreateValidator : AbstractValidator<ProductInsertModel>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x).Must(HaveConsistentTemplateAndProperty);
        RuleFor(x => x.CategoryId).NotNull();
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
}