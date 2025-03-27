using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;

public class CreateSaleProfile : Profile
{

    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemDtoRequest, CreateSaleItemDto>();
        CreateMap<CreateSaleResult, CreateSaleResponse>();
    }
    
}