namespace Ambev.DeveloperEvaluation.Common.Security;

/// <summary>
/// Defines the contract for representing a Sale in the system.
/// </summary>
public interface ISale
{
    /// <summary>
    /// Gets the unique identifier of the Sale entity. 
    /// </summary>
    /// <returns>The Sale ID as a string.</returns>
    public string Id { get; }
}