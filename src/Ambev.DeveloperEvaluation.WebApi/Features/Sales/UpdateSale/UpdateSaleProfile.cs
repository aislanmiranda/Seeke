using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using static Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale.UpdateSaleRequest;
using static Ambev.DeveloperEvaluation.Application.Sales.UpdateSale.UpdateSaleCommand;
using static Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale.UpdateSaleResponse;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Application and API UpdateSale responses
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();

        CreateMap<UpdateSaleResult, UpdateSaleResponse>()
              .ForMember(dest => dest.SaleId, src
                    => src.MapFrom(src => src.Id));
        CreateMap<UpdateSaleItemsResult, UpdateSaleItemResponse>();
    }
}
