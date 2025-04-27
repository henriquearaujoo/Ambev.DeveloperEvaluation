using Ambev.DeveloperEvaluation.Application.Sale.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly ILogger<DeleteSaleCommandHandler> _logger = Substitute.For<ILogger<DeleteSaleCommandHandler>>();

    [Fact(DisplayName = "DeleteSale should succeed when valid request is provided")]
    public async Task DeleteSale_Should_Succeed_When_ValidRequest()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId).Returns(new Sale { Id = saleId });

        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(saleId);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _repository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "DeleteSale should throw KeyNotFoundException when sale not found")]
    public async Task DeleteSale_Should_Throw_When_Sale_Not_Found()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId).Returns((Sale?)null);

        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(saleId);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact(DisplayName = "DeleteSale should throw ValidationException when Id is empty")]
    public async Task DeleteSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        // Arrange
        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(Guid.Empty);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}