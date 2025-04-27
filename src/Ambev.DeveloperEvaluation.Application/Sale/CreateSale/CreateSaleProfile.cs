using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale;

public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Domain.Entities.Sale>();
        CreateMap<Domain.Entities.Sale, CreateSaleCommand>().ReverseMap();
        CreateMap<SaleItem, CreateSaleItemDto>().ReverseMap();
        CreateMap<Domain.Entities.Sale, CreateSaleResult>();
    }
}