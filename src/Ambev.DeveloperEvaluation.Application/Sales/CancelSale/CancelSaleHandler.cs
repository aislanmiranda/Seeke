using MediatR;
using FluentValidation;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer not found");

        var CancelSale = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (CancelSale == null)
            throw new InvalidOperationException("Sale not found");

        CancelSale.CancelSale();

        var CancelSaleResponse = await _saleRepository.UpdateAsync(CancelSale!, cancellationToken);

        var result = _mapper.Map<CancelSaleResult>(CancelSaleResponse);

        return result;
    }
}