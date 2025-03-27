namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CancelSale;

public class CancelSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to delete
    /// </summary>
    public Guid Id { get; set; }
}