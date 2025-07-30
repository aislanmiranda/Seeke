using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using static Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.UpdateSaleCommand;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale operation
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<Sale, UpdateSaleResult>();
        CreateMap<SaleItem, UpdateSaleItemsResult>();        
    }
}

