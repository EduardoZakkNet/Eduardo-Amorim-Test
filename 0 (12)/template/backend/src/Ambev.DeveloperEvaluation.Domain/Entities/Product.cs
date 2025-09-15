using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product: BaseEntity, IInformationAudit
{
    /// <summary>
    /// The unique identifier of the Product of Sale
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// The product full name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Quantities of products in a sale
    /// </summary>
    public int Quantities { get; set; } = 0;
    
    /// <summary>
    /// The Unit price of sale
    /// </summary>
    public decimal UnitPrice { get; set; } = 0m;
    
    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// The Discounts in a sale
    /// </summary>
    public decimal Discounts { get; set; } = 0m;

    /// <summary>
    /// Total amount for each item
    /// </summary>
    public decimal TotalSaleAmount { get; set; } = 0m;
}