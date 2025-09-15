using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator: AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Product Name is required.");
        
        RuleFor(product => product.Quantities)
            .NotNull().WithMessage("Product Quantities is required.")
            .GreaterThan(0).WithMessage("Product Quantities must be greater than zero.");
        
        RuleFor(product => product.UnitPrice)
            .NotNull().WithMessage("Product Unit Price is required.")
            .GreaterThan(0).WithMessage("Product Unit Price must be greater than zero.");
    }
}