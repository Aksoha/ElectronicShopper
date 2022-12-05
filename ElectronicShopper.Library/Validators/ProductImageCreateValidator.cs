using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class ProductImageCreateValidator : AbstractValidator<MemoryImageModel>
{
    private static readonly List<string> Extensions = new() { ".jpg", ".jpeg", ".png", ".svg"};

    public ProductImageCreateValidator()
    {
        RuleFor(x => x.Extension).Must(x => Extensions.Contains(x))
            .WithMessage($"Extension is unsupported. Supported extensions {string.Join(",", Extensions)}");
        Transform(x => x.Name, s => s.Trim()).NotEmpty();
    }
}