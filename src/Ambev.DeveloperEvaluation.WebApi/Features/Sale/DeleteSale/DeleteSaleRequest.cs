namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSale;

public class DeleteSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; set; }
}