using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Fact]
    public void SaleItem_Should_Calculate_Total_Correctly()
    {
        var item = new SaleItem
        {
            Quantity = 3,
            UnitPrice = 10.0m,
            Discount = 5.0m 
        };

        Assert.Equal(25.0m, item.Total);
    }

    [Fact]
    public void SaleItem_Should_Generate_Id_And_CreatedAt_On_Construction()
    {
        var item = new SaleItem();

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.True((DateTime.UtcNow - item.CreatedAt).TotalSeconds < 2);
    }
}