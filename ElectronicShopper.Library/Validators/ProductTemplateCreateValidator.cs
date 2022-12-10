using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="ProductTemplateModel" /> can be used for inserting new product
///     template into the database.
/// </summary>
public class ProductTemplateCreateValidator : AbstractValidator<ProductTemplateModel>
{
    public ProductTemplateCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleForEach(x => x.Properties).NotNull().WithMessage("Property must not be empty");
    }
}