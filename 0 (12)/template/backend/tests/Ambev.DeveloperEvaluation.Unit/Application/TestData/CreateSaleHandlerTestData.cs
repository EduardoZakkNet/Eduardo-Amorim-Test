using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid User entities.
    /// The generated users will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleDate, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today))
        .RuleFor(s => s.Customer, f  => GenerateCustomer())
        .RuleFor(s => s.TotalSaleAmount, f => f.Random.Decimal(100000000, 999999999))
        .RuleFor(s => s.Branch, f  => GenerateBranch())
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
        .RuleFor(c => c.Status, f => f.PickRandom(SaleStatus.Active, SaleStatus.Suspended))
        .RuleFor(s => s.Products, f  => GenerateProducts(f.Random.Int(1, 5)));

    /// <summary>
    /// Generates a Random Branch that be use in a Sale.
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A new Branch</returns>
    private static Branch GenerateBranch()
    {
        return new Faker<Branch>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => f.Random.String2(12))
            .RuleFor(b => b.CreatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today))
            .RuleFor(b => b.UpdatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today));
    }
    
    /// <summary>
    /// Generates a Random Customer that be use in a Sale.
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A new Customer</returns>
    private static Customer GenerateCustomer()
    {
        return new Faker<Customer>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Random.String2(12))
            .RuleFor(c => c.Status, f => f.PickRandom(CustomerStatus.Active, CustomerStatus.Suspended))
            .RuleFor(c => c.CreatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today))
            .RuleFor(c => c.UpdatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today));
    }
    
    /// <summary>
    /// Generates a Random Lisf of Products that be use in a Sale.
    /// that meet the system's validation requirements.
    /// </summary>
    ///<see cref="count"/> Quantities of products will gerated
    /// <returns>A new list of Products</returns>
    private static List<Product> GenerateProducts(int count)
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Quantities, f => f.Random.Number(10, 20))
            .RuleFor(p => p.UnitPrice, f => f.Random.Decimal(100000000, 999999999))
            .RuleFor(p => p.CreatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today))
            .RuleFor(p => p.UpdatedAt, f => f.Date.Between(new DateTime(1990, 1, 1), DateTime.Today))
            .RuleFor(p => p.Discounts, f => f.Random.Decimal(100000000, 999999999))
            .RuleFor(p => p.TotalSaleAmount, f => f.Random.Decimal(100000000, 999999999));
        return faker.Generate(count);
    }
    
    /// <summary>
    /// Generates a valid User entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid User entity with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }
}