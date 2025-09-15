using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for Sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
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