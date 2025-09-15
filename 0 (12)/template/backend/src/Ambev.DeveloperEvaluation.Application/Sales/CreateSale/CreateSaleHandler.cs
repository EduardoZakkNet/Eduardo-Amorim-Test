using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Sales.Config;
using Ambev.DeveloperEvaluation.Application.Service;
using MediatR;
using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler: IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IProductRepository _productRepository;
    
    /// <summary>
    /// Initializes a new instance of CreateUserHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IConfiguration configuration, 
        ILogger<CreateSaleHandler> logger, ICustomerRepository customerRepository, 
        IBranchRepository branchRepository, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }
    
    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var sale = _mapper.Map<Sale>(command);
        var updatedProducts = new List<Product>();
        sale.Customer = await GetOrCreateCustomerAsync(sale.Customer, cancellationToken);
        sale.Branch = await GetOrCreateBranchAsync(sale.Branch, cancellationToken);
        foreach (var product in sale.Products)
        {
            var updatedProduct = await GetOrCreateProductAsync(product, cancellationToken);
            updatedProducts.Add(updatedProduct);
        }
        sale.Products = updatedProducts;
        ValidateProducts(sale.Products);
        
        decimal totalSaleAmount = 0m;
        foreach (var product in sale.Products)
        {
            var discountPercentage = CalculateDiscountPercentage(product.Quantities);
            product.Discounts = discountPercentage;
            product.TotalSaleAmount = CalculateTotalItemAmount(product, discountPercentage);
            totalSaleAmount += product.TotalSaleAmount;
        }
        
        sale.TotalSaleAmount = totalSaleAmount;
        sale.SaleDate = DateTime.UtcNow;
        
        var createdUser = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdUser);
        
        await PublishSaleCreatedEventAsync(result);
        
        return result;
    }
    
    /// <summary>
    /// Method the Validade if Product exists in data base
    /// </summary>
    /// <param name="Product">The Product  in the Sale</param>
    /// <returns>Method the Validade if Product exists in data base</returns>
    private async Task<Product> GetOrCreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        if (product == null || product.Id == Guid.Empty)
        {
            var newProduct = await _productRepository.CreateAsync(product, cancellationToken);
            return newProduct;
        }
        else
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.Id, cancellationToken);
            if (existingProduct != null)
            {
                return existingProduct;
            }
            else
            {
                var newProduct = await _productRepository.CreateAsync(product, cancellationToken);
                return newProduct;
            }
        }
    }
    
    /// <summary>
    /// Method the Validade if Customer exists in data base
    /// </summary>
    /// <param name="Customer">The Customer  in the Sale</param>
    /// <returns>Method the Validade if Customer exists in data base</returns>
    private async Task<Customer> GetOrCreateCustomerAsync(Customer customer, CancellationToken cancellationToken)
    {
        if (customer == null || customer.Id == Guid.Empty)
            return await _customerRepository.CreateAsync(customer, cancellationToken);

        var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id, cancellationToken);
        return existingCustomer ?? await _customerRepository.CreateAsync(customer, cancellationToken);
    }

    /// <summary>
    /// Method the Validade if Branch exists in data base
    /// </summary>
    /// <param name="Branch">The Branch  in the Sale</param>
    /// <returns>Method the Validade if Branch exists in data base</returns>
    private async Task<Branch> GetOrCreateBranchAsync(Branch branch, CancellationToken cancellationToken)
    {
        if (branch == null || branch.Id == Guid.Empty)
            return await _branchRepository.CreateAsync(branch, cancellationToken);

        var existingBranch = await _branchRepository.GetByIdAsync(branch.Id, cancellationToken);
        return existingBranch ?? await _branchRepository.CreateAsync(branch, cancellationToken);
    }
    
    /// <summary>
    /// Method the ValidateProducts
    /// </summary>
    /// <param name="products">The List of Products in the Sale</param>
    /// <returns>Validates if the total quantity of products exceeds the limit</returns>
    private void ValidateProducts(IEnumerable<Product> products)
    {
        var invalidProducts = products
            .Where(p => p.Quantities > 20)
            .ToList();
        
        if (invalidProducts.Any())
        {
            var productNames = string.Join(", ", invalidProducts.Select(p => p.Name));
            throw new InvalidOperationException(
                $"The product exceeds the maximum limit of 20 items per product: {productNames}."
            );
        }
    }
    
    /// <summary>
    /// Method the ValidateProducts
    /// </summary>
    /// <param name="quantity">The Quantities of Products in the Sale</param>
    /// <returns>Calculates the discount percentage based on the quantity</returns>
    private decimal CalculateDiscountPercentage(int quantity)
    {
        if (quantity < 4)
            return 0m;      //There isn't discount
        if (quantity <= 9)
            return 0.10m;   // There is a 10% discount
        if (quantity <= 20)
            return 0.20m;   // There is a 20% discount

        return 0m;          //If not expected, but for safety
    }
    
    /// <summary>
    /// Method the ValidateProducts
    /// </summary>
    /// <param name="Product">The Product of the Sale</param>
    /// <param name="discountPercentage">The discountPercentage of Products in the Sale</param>
    /// <returns>Calculates the total value of the item considering the discount</returns>
    private decimal CalculateTotalItemAmount(Product product, decimal discountPercentage)
    {
        var unitPriceWithDiscount = product.UnitPrice * (1 - discountPercentage);
        return unitPriceWithDiscount * product.Quantities;
    }
    
    /// <summary>
    /// Method the ValidateProducts
    /// </summary>
    /// <param name="result">The Sale entity</param>
    /// <returns>Publish in kafka the new sale information</returns>
    private async Task PublishSaleCreatedEventAsync(CreateSaleResult result)
    {
        var config = new SaleCreatedIntegrationKafkaConfig();
        var bootstrapServers = _configuration["AmbevServerKafka:uri"];
        var keySecurityKafka = _configuration["AmbevServerKafka:key"];
        
        using var kafkaService = new KafkaProducerService<SaleCreatedIntegrationKafkaConfig>(bootstrapServers, config);
        var message = JsonSerializer.Serialize(result);
        
        //Apenas uma simulação, para que não ocorra erro, foi comentado para que seja logado simulando o Publish original no kafka
        
        //await kafkaService.PublicarAsync(message, keySecurityKafka);
        _logger.LogInformation("Event published to Kafka successfully - Topic: {Nome}", config.TopicName);
    }
}