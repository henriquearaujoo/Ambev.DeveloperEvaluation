namespace Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;

public class UpdateSaleItemDto
{
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

}