using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using static Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale.CancelSaleResponse;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Profile for mapping between Application and API CancelSale responses
/// </summary>
public class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelSale feature
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleRequest, CancelSaleCommand>();

        CreateMap<CancelSaleResult, CancelSaleResponse>()
              .ForMember(dest => dest.SaleId, src
                    => src.MapFrom(src => src.Id));
        CreateMap<CancelSaleItemsResult, CancelSaleItemResponse>();
    }
}
