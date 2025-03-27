using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.DeleteSale;

public class DeleteSaleCommand : IRequest
{
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// The unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; }
}