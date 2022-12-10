using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="MemoryImageModel" /> can be used for inserting new image into
///     the database.
/// </summary>
public class ProductImageCreateValidator : AbstractValidator<MemoryImageModel>
{
    /// <summary>
    ///     Supported file extensions.
    /// </summary>
    private static readonly List<string> Extensions = new() { ".jpg", ".jpeg", ".png", ".svg" };

    public ProductImageCreateValidator()
    {
        RuleFor(x => x.Extension).Must(x => Extensions.Contains(x))
            .WithMessage($"Extension is unsupported. Supported extensions {string.Join(",", Extensions)}");
        Transform(x => x.Name, s => s.Trim()).NotEmpty();
    }
}