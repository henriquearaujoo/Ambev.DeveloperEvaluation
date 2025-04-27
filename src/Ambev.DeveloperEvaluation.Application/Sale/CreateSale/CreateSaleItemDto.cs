namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale;

/// <summary>
/// DTO representing a sale item in the create sale request.
/// </summary>
public class CreateSaleItemDto
{
    /// <summary>
    /// Gets or sets the product name or identifier.
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}