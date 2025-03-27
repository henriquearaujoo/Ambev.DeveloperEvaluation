using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale;

public class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        CreateMap<Domain.Entities.Sale, GetSaleResult>();
        CreateMap<Domain.Entities.SaleItem, GetSaleItemDtoResult>();
    }
}