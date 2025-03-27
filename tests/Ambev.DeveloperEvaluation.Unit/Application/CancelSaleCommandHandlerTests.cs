using Ambev.DeveloperEvaluation.Application.Sale.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly ILogger<CancelSaleCommandHandler> _logger = Substitute.For<ILogger<CancelSaleCommandHandler>>();

    [Fact]
    public async Task CancelSale_Should_Succeed_When_ValidRequest()
    {
        var command = new CancelSaleCommand(Guid.NewGuid());
        var handler = new CancelSaleCommandHandler(_repository, _logger);

        await handler.Handle(command, CancellationToken.None);

        await _repository.Received(1).CancelAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        var command = new CancelSaleCommand(Guid.Empty);
        var handler = new CancelSaleCommandHandler(_repository, _logger);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}