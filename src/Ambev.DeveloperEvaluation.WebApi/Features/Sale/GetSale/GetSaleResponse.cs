using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.GetSale;

public class GetSaleResponse
{
    /// <summary>
    /// Unique number identifying the sale.
    /// </summary>
    public string SaleNumber { get; set; }

    /// <summary>
    /// Date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Name or identifier of the customer who made the purchase.
    /// </summary>
    public string Customer { get; set; }

    /// <summary>
    /// Branch where the sale was performed.
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Indicates whether the sale was cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Total amount of the sale, calculated from its items.
    /// </summary>
    public decimal TotalAmount => Items.Sum(i => i.Total);

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// List of items included in the sale.
    /// </summary>
    public List<GetSaleItemDtoResponse> Items { get; set; } = new();
}