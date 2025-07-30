using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale operation
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<Sale, CancelSaleResult>();
        CreateMap<SaleItem, CancelSaleItemsResult>();        
    }
}

