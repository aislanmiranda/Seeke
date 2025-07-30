using MediatR;
using FluentValidation;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Search client
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer not found");

        // Busca branch name
        var branch = await GetCurrentBranchAsync();

        // Group Products By Id
        List<SaleItem> groupedItems = await GroupProductsById(command);

        // Create Sale entity
        var createSale = new Sale(customer.Id, customer.CustomerName!, branch.Id, branch.BranchName!);

        // Adiciona os itens via método da entidade (mantendo lógica de domínio centralizada)
        createSale.AddItems(groupedItems);

        // Salva a venda no repositório
        var createSaleResponse = await _saleRepository.CreateAsync(createSale);

        var result = _mapper.Map<CreateSaleResult>(createSaleResponse);

        return result;
    }

    private async Task<List<SaleItem>> GroupProductsById(CreateSaleCommand command)
    {
        // 1. Get unique product IDs
        var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();

        // 2. Search for all products in a single call
        var products = await _productRepository.GetByIdsAsync(productIds);

        // 3. Create a dictionary for quick access to the product name
        var productDictionary = products.ToDictionary(p => p.Id, p => p.ProductName);

        // 4. Group and include the product name
        var groupedItems = command.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new SaleItem
            (
                productId: g.Key,
                quantity: g.Sum(x => x.Quantity),
                unitPrice: g.First().UnitPrice,
                productName: productDictionary.TryGetValue(g.Key, out string? name) ? name! : "N/A"
            ))
            .ToList();
        return groupedItems;
    }

    private Task<Branch> GetCurrentBranchAsync()
    {
        // Simulation to return a branch
        var branch = new Branch(Guid.NewGuid(), "São Paulo - Unidade Central");

        return Task.FromResult(branch);
    }
}



