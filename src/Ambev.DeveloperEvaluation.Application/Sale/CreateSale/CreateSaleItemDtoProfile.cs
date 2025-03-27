using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale;

public class CreateSaleItemDtoProfile : Profile
{
    public CreateSaleItemDtoProfile()
    {
        CreateMap<CreateSaleItemDto, Domain.Entities.SaleItem>();
    }
}