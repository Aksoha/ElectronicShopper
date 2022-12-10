using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="CategoryCreateModel" /> can be used for inserting new category into
///     the database.
/// </summary>
public class CategoryCreateValidator : AbstractValidator<CategoryCreateModel>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Category name must not be empty");
    }
}