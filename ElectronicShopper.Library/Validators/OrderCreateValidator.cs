using ElectronicShopper.Library.Models;
using FluentValidation;

namespace ElectronicShopper.Library.Validators;

public class OrderCreateValidator : AbstractValidator<OrderModel>
{
    public OrderCreateValidator()
    {
        RuleFor(x => x.PurchasedProducts.Count).GreaterThan(0);
        RuleForEach(x => x.PurchasedProducts).SetValidator(new OrderDetailCreateValidator());
    }
}