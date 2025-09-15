using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
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