using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;

public class UpdateSaleProfile : Profile
{
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleCommand, Domain.Entities.Sale>();
        CreateMap<Domain.Entities.Sale, UpdateSaleResult>();
        CreateMap<Domain.Entities.SaleItem, UpdateSaleItemDtoResult>();
    }
}