using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Fact(DisplayName = "SaleItem should calculate total correctly")]
    public void SaleItem_Should_Calculate_Total_Correctly()
    {
        // Arrange
        var item = new SaleItem
        {
            Quantity = 3,
            UnitPrice = 10.0m,
            Discount = 5.0m
        };

        // Act & Assert
        item.Total.Should().Be(25.0m);
    }

    [Fact(DisplayName = "SaleItem should generate Id and CreatedAt on construction")]
    public void SaleItem_Should_Generate_Id_And_CreatedAt_On_Construction()
    {
        // Act
        var item = new SaleItem();

        // Assert
        item.Id.Should().NotBe(Guid.Empty);
        item.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
}