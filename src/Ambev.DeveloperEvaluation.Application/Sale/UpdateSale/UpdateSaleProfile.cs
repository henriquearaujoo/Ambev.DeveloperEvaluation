using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleResult, Domain.Entities.Sale>().ReverseMap();
        CreateMap<UpdateSaleItemDtoResult, SaleItem>().ReverseMap();
    }
}