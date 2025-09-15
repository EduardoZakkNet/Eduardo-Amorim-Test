using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class TotalSalesAmountValidator : AbstractValidator<Decimal>
{
    public TotalSalesAmountValidator()
    {
        RuleFor(value => value)
            .NotEmpty().WithMessage("The Total Sales Amount is required.")
            .GreaterThan(0).WithMessage("The Total Sales Amount must be positive.");
    }
}