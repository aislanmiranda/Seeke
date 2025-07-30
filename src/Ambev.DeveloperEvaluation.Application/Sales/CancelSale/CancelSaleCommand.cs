using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    public Guid CustomerId { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
}
