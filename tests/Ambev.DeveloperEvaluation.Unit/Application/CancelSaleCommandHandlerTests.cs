using Ambev.DeveloperEvaluation.Application.Sale.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly ILogger<CancelSaleCommandHandler> _logger = Substitute.For<ILogger<CancelSaleCommandHandler>>();

    [Fact(DisplayName = "CancelSale should succeed when valid request is provided")]
    public async Task CancelSale_Should_Succeed_When_ValidRequest()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid());
        var handler = new CancelSaleCommandHandler(_repository, _logger);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _repository.Received(1).CancelAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "CancelSale should throw ValidationException when Id is empty")]
    public async Task CancelSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.Empty);
        var handler = new CancelSaleCommandHandler(_repository, _logger);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}