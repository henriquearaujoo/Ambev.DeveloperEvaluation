using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<GetSaleResult, Domain.Entities.Sale>().ReverseMap();
        CreateMap<GetSaleItemDtoResult, SaleItem>().ReverseMap();
    }
}