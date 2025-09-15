using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer)
            .NotEmpty().WithMessage("The Customer is required.");
        
        RuleFor(customer => customer.Name)
            .NotEmpty().WithMessage("The Customer Name entered is invalid")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");
    }
}