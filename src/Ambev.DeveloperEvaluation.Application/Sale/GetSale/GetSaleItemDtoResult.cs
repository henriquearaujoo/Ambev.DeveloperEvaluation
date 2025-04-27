namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale;

public class GetSaleItemDtoResult
{
    /// <summary>
    /// Unique identifier for the sale item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product name or identifier.
    /// </summary>
    public string Product { get; set; }

    /// <summary>
    /// Quantity of the product sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for this item after applying discount.
    /// </summary>
    public decimal Total => Math.Round((UnitPrice * Quantity) - Discount, 2);

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the user's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}