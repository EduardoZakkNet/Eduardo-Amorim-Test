using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the Date. Must be date valid.
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the Customer. Must be Customer valid.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Gets or sets the Total Sales Amount. Must be valid and greater than zero.
    /// </summary>
    public decimal TotalSaleAmount { get; set; } = 0m;
    
    /// <summary>
    /// Gets or sets the Branch. Must be Branch valid.
    /// </summary>
    public Branch Branch { get; set; }
    
    /// <summary>
    /// Gets or sets the Product. Must be Product valid.
    /// </summary>
    public IEnumerable<Product> Products { get; set; }
}