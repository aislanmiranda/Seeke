
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleRequest
{
    public string SaleNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}
