namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Domain service responsible for applying business rules related to sales.
/// </summary>
public class SaleService : ISaleService
{
    /// <summary>
    /// Applies discounts to the sale items based on total quantity per product.
    /// </summary>
    /// <param name="sale">The sale entity to apply discounts to.</param>
    public void ApplyDiscounts(Sale sale)
    {
        var productGroups = sale.Items
            .GroupBy(i => i.Product)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

        foreach (var item in sale.Items)
        {
            var totalQuantityForProduct = productGroups[item.Product];

            if (totalQuantityForProduct > 20)
                throw new InvalidOperationException($"Cannot sell more than 20 items of product: {item.Product}.");

            if (totalQuantityForProduct >= 10)
                item.Discount = Math.Round(item.UnitPrice * item.Quantity * 0.2m, 2);
            else if (totalQuantityForProduct >= 4)
                item.Discount = Math.Round(item.UnitPrice * item.Quantity * 0.1m, 2);
            else
                item.Discount = 0;
        }
    }
}