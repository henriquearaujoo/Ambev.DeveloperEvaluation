using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Total amount should be the sum of all item totals")]
    public void TotalAmount_Should_Be_Sum_Of_Item_Totals()
    {
        // Arrange
        var sale = new Sale
        {
            Items = new List<SaleItem>
            {
                new() { Quantity = 2, UnitPrice = 10.0m, Discount = 1.0m }, 
                new() { Quantity = 1, UnitPrice = 5.0m, Discount = 0.0m }   
            }
        };

        // Act & Assert
        sale.TotalAmount.Should().Be(24.0m);
    }

    [Fact(DisplayName = "Cancel should set IsCancelled true and update UpdatedAt")]
    public void Cancel_Should_Set_IsCancelled_True_And_Update_UpdatedAt()
    {
        // Arrange
        var sale = new Sale { IsCancelled = false };

        // Act
        sale.Cancel();

        // Assert
        sale.IsCancelled.Should().BeTrue();
        sale.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Cancel should throw when already cancelled")]
    public void Cancel_Should_Throw_When_Already_Cancelled()
    {
        // Arrange
        var sale = new Sale { IsCancelled = true };

        // Act
        Action act = () => sale.Cancel();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Sale is already cancelled.");
    }

    [Fact(DisplayName = "Sale should generate Id and CreatedAt on construction")]
    public void Sale_Should_Generate_Id_And_CreatedAt_On_Construction()
    {
        // Act
        var sale = new Sale();

        // Assert
        sale.Id.Should().NotBe(Guid.Empty);
        sale.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
}