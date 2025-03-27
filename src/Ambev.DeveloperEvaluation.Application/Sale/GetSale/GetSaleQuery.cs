using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale;

public class GetSaleQuery : IRequest<GetSaleResult>
{
    public GetSaleQuery(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// The unique identifier of the sale to retrieve
    /// </summary>
    public Guid Id { get; }
}