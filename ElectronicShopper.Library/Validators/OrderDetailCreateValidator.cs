using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class OrderDetailCreateValidator : AbstractValidator<OrderDetailModel>
{
    public OrderDetailCreateValidator()
    {
        RuleFor(x => x.ProductId).NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.PricePerItem).GreaterThan(0);
        
    }
}