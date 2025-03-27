using Ambev.DeveloperEvaluation.Application.Sale.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;

public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale feature
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Guid, GetSaleQuery>()
            .ConstructUsing(id => new GetSaleQuery(id));
        CreateMap<GetSaleItemDtoResult, GetSaleItemDtoResponse>();
        CreateMap<GetSaleResult, GetSaleResponse>();
        
    }
}