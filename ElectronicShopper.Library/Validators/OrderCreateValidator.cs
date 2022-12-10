using FluentValidation;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A set of rules validating whether <see cref="OrderModel" /> can be used for inserting new order into
///     the database.
/// </summary>
public class OrderCreateValidator : AbstractValidator<OrderModel>
{
    public OrderCreateValidator(IValidator<OrderDetailModel> detailValidator)
    {
        RuleFor(x => x.PurchasedProducts.Count).GreaterThan(0)
            .WithMessage("order should contain at least one product");
        RuleForEach(x => x.PurchasedProducts).SetValidator(detailValidator);
    }
}