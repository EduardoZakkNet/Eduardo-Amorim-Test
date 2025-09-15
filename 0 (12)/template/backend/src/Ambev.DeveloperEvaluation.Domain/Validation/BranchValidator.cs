using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class BranchValidator : AbstractValidator<Branch>
{
    public BranchValidator()
    {
        RuleFor(branch => branch)
            .NotEmpty().WithMessage("The Branch is required.");
        
        RuleFor(branch => branch.Name)
            .NotEmpty().WithMessage("The Branch Name entered is invalid")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(200).WithMessage("Name cannot be longer than 200 characters.");
    }
}