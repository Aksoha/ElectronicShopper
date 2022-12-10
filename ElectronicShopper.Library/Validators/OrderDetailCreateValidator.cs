using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="OrderDetailModel" /> can be used for inserting new order details into
///     the database.
/// </summary>
public class OrderDetailCreateValidator : AbstractValidator<OrderDetailModel>
{
    public OrderDetailCreateValidator()
    {
        RuleFor(x => x.ProductId).NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0)
            .WithMessage("quantity of purchased product must be positive");
        RuleFor(x => x.PricePerItem).GreaterThan(0)
            .WithMessage("price of the product must be positive. A \"freebie\" should cost 0.01");
        
    }
}