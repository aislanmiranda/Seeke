
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    public string SaleNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public List<UpdateSaleItemRequest> Items { get; set; } = new();
    
    public class UpdateSaleItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
