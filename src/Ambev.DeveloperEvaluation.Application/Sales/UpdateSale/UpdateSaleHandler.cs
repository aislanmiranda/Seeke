using MediatR;
using FluentValidation;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public UpdateSaleHandler(
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

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Search client
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer not found");

        var updateSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (updateSale == null)
            throw new InvalidOperationException("Sale not found");

        // Group Products By Id
        List<SaleItem> groupedItems = await GroupProductsById(command);

        updateSale.UpdateCustomer(customer.Id, customer.CustomerName!);
        updateSale.UpdateItems(groupedItems);

        var updateSaleResponse = await _saleRepository.UpdateAsync(updateSale!, cancellationToken);

        var result = _mapper.Map<UpdateSaleResult>(updateSaleResponse);

        return result;
    }

    private async Task<List<SaleItem>> GroupProductsById(UpdateSaleCommand command)
    {
        // 1. Get unique product IDs
        var productIds = command.Items.Select(i => i.ProductId).Distinct().ToList();

        // 2. Search for all products in a single call
        var products = await _productRepository.GetByIdsAsync(productIds);

        // 3. Update a dictionary for quick access to the product name
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
}



