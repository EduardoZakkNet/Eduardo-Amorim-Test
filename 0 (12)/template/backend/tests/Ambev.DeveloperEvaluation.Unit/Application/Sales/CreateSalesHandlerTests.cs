using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSalesHandlerTests"/> class.
/// </summary>
public class CreateSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IBranchRepository _branchRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _configuration = Substitute.For<IConfiguration>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _configuration, _logger, 
            _customerRepository, _branchRepository, _productRepository);
    }
    
    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var expectedMessage = "Event published to Kafka successfully";
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleDate = command.SaleDate,
            Customer = command.Customer,
            TotalSaleAmount = command.TotalSaleAmount,
            Branch = command.Branch,
            Products = command.Products,
            IsCancelled = command.IsCancelled,
            Status = command.Status
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
        };


        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(sale.Customer);
        
        _branchRepository.CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>())
            .Returns(sale.Branch);
        
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Product>());
        
        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        
        // Assert
        _logger.Received(1);
        
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
    
    /// <summary>
    /// products exceeding limit When creating sale Then throws InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given products exceeding limit When creating sale Then throws InvalidOperationException")]
    public async Task Handle_ProductExceedsLimit_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        
        var updatedProducts = command.Products
            .Select(p => 
            {
                p.Quantities = 50;
                return p;
            }).ToList();
        
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleDate = command.SaleDate,
            Customer = command.Customer,
            TotalSaleAmount = command.TotalSaleAmount,
            Branch = command.Branch,
            Products = updatedProducts,
            IsCancelled = command.IsCancelled,
            Status = command.Status
        };
        
        _mapper.Map<Sale>(command).Returns(sale);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        
        _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
            .Returns(sale.Customer);
        
        _branchRepository.CreateAsync(Arg.Any<Branch>(), Arg.Any<CancellationToken>())
            .Returns(sale.Branch);

        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Product>());
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _handler.Handle(command, CancellationToken.None);
        });
    }
    
    /// <summary>
    /// Tests that an invalid sale creation request throws a ArgumentNullException.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating user Then throws ArgumentNullException")]
    public async Task Handle_InvalidRequest_ArgumentNullException()
    {
        // Given
        var command = new CreateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}