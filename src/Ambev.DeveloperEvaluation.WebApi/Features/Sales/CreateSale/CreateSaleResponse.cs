
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleResponse
{
    public Guid SaleId { get; set; }

    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public string BranchName { get; private set; } = string.Empty;
    public List<CreateSaleItemResponse>? Items { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCanceled { get; private set; }

    public class CreateSaleItemResponse
    {
        public Guid SaleId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total { get; private set; }
    }
}