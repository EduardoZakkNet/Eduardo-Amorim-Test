using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity, ISale
{
    /// <summary>
    /// Gets the Date Sale.
    /// Must not be null or empty and valid date.
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets Customer of the Sale.
    /// Must not be null or empty and valid Customer.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Gets Total Sales Amount of the Sale.
    /// Must not be null or empty and valid value.
    /// </summary>
    public decimal TotalSaleAmount { get; set; } = 0m;
    
    /// <summary>
    /// Gets Branch of the Sale.
    /// Must not be null or empty and valid.
    /// </summary>
    public Branch Branch { get; set; }
    
    /// <summary>
    /// Gets List of Product of the Sale.
    /// Must not be null or empty and each product valid.
    /// </summary>
    public IEnumerable<Product> Products { get; set; }
    
    /// <summary>
    /// Gets Branch a sale is canceled/not canceled
    /// Must not be null or empty.
    /// </summary>
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// Gets the sale's current status.
    /// Indicates whether the user is active, inactive, or blocked in the system.
    /// </summary>
    public SaleStatus Status { get; set; }
    
    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the sale's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Gets the unique identifier of the sale.
    /// </summary>
    /// <returns>The sale's ID as a string.</returns>
    string ISale.Id => Id.ToString();
    
    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Performs validation of the Sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">Username format and length</list>
    /// <list type="bullet">Email format</list>
    /// <list type="bullet">Phone number format</list>
    /// <list type="bullet">Password complexity requirements</list>
    /// <list type="bullet">Role validity</list>
    /// 
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Activates the sale account.
    /// Changes the sale's status to Active.
    /// </summary>
    public void Activate()
    {
        Status = SaleStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Deactivates the sale account.
    /// Changes the sale's status to Inactive.
    /// </summary>
    public void Deactivate()
    {
        Status = SaleStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Blocks the sale account.
    /// Changes the sale's status to Blocked.
    /// </summary>
    public void Suspend()
    {
        Status = SaleStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }
}