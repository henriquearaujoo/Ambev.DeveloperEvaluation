using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void TotalAmount_Should_Be_Sum_Of_Item_Totals()
    {
        var sale = new Sale
        {
            Items = new List<SaleItem>
            {
                new() { Quantity = 2, UnitPrice = 10.0m, Discount = 1.0m }, 
                new() { Quantity = 1, UnitPrice = 5.0m, Discount = 0.0m }   
            }
        };

        Assert.Equal(24.0m, sale.TotalAmount);
    }

    [Fact]
    public void Cancel_Should_Set_IsCancelled_True_And_Update_UpdatedAt()
    {
        var sale = new Sale { IsCancelled = false };

        sale.Cancel();

        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }

    [Fact]
    public void Cancel_Should_Throw_When_Already_Cancelled()
    {
        var sale = new Sale { IsCancelled = true };

        var ex = Assert.Throws<InvalidOperationException>(() => sale.Cancel());
        Assert.Equal("Sale is already cancelled.", ex.Message);
    }

    [Fact]
    public void Sale_Should_Generate_Id_And_CreatedAt_On_Construction()
    {
        var sale = new Sale();

        Assert.NotEqual(Guid.Empty, sale.Id);
        Assert.True((DateTime.UtcNow - sale.CreatedAt).TotalSeconds < 2);
    }
}