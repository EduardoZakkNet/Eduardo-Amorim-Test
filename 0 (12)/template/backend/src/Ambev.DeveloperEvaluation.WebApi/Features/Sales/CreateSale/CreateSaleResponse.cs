using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// API response model for CreateSale operation
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// Sale number
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// The customer role in the system
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// The Total sale amount
    /// </summary>
    public decimal TotalSaleAmount { get; set; } = 0m;
    
    /// <summary>
    /// Branch where the sale was made
    /// </summary>
    public Branch Branch { get; set; }
    
    /// <summary>
    /// Products of the sale
    /// </summary>
    public IEnumerable<Product> Products { get; set; }
    
    /// <summary>
    /// Defines whether a sale is canceled/not canceled
    /// </summary>
    public bool IsCancelled { get; set; }
}