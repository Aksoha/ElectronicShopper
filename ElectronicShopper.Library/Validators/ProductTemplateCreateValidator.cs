using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
/// Validates <see cref="ProductTemplateModel"/> for insert into database.
/// </summary>
public class ProductTemplateCreateValidator : AbstractValidator<ProductTemplateModel>
{
    public ProductTemplateCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty");
        RuleForEach(x => x.Properties).NotNull().WithMessage("Property must not be empty");
    }
}