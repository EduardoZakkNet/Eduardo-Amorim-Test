using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleDate).SetValidator(new DateValidator());
        RuleFor(sale => sale.Customer).SetValidator(new CustomerValidator());
        RuleFor(sale => sale.TotalSaleAmount).SetValidator(new TotalSalesAmountValidator());
        RuleFor(sale => sale.Branch).SetValidator(new BranchValidator());

        RuleFor(sale => sale.Products) .NotNull().WithMessage("The product list cannot be null.") 
            .Must(products => products.Any()).WithMessage("The product list cannot be empty.");
        
        RuleForEach(sale => sale.Products).SetValidator(new ProductValidator());
    }
}