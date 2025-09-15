using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Customer : BaseEntity, IInformationAudit
{
    /// <summary>
    /// The unique identifier of the Customer of Sale
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// The customer full name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the customer current status.
    /// Indicates whether the user is active, inactive, or blocked in the system.
    /// </summary>
    public CustomerStatus Status { get; set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}