
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<CreateSaleResult> // retorna o Id da venda criada
{
    public Guid CustomerId { get; set; }
    public List<SaleItemCommand> Items { get; set; } = new();

    public class SaleItemCommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
