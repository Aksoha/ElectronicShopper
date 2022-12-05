using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateModel>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Category name must not be empty");
    }
}