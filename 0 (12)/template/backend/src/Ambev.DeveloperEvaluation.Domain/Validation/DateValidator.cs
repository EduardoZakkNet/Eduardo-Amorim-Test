using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class DateValidator : AbstractValidator<DateTime>
{
    public DateValidator()
    {
        RuleFor(date => date)
            .NotEmpty().WithMessage("The date is required.")
            .Must(BeValidDate).WithMessage("The provided date is not valid.")
            .Must(BeAfter1990).WithMessage("The date must be after 01/01/1990.")
            .Must(NotBeFutureDate).WithMessage("The date cannot be in the future.");
    }

    private bool BeValidDate(DateTime date)
    {
        return date != default(DateTime);
    }

    private bool BeAfter1990(DateTime date)
    {
        var minDate = new DateTime(1990, 1, 1);
        return date > minDate;
    }

    private bool NotBeFutureDate(DateTime date)
    {
        return date <= DateTime.Now;
    }
}