using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using static Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale.CreateSaleRequest;
using static Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleCommand;
using static Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale.CreateSaleResponse;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Application and API CreateSale responses
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<SaleItemRequest, SaleItemCommand>();

        CreateMap<CreateSaleResult, CreateSaleResponse>()
              .ForMember(dest => dest.SaleId, src
                    => src.MapFrom(src => src.Id));
        CreateMap<CreateSaleItemsResult, CreateSaleItemResponse>();
    }
}
