using Ambev.DeveloperEvaluation.Application.Sale.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class DeleteSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly ILogger<DeleteSaleCommandHandler> _logger = Substitute.For<ILogger<DeleteSaleCommandHandler>>();

    [Fact]
    public async Task DeleteSale_Should_Succeed_When_ValidRequest()
    {
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId).Returns(new Sale { Id = saleId });

        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(saleId);

        await handler.Handle(command, CancellationToken.None);

        await _repository.Received(1).DeleteAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteSale_Should_Throw_When_Sale_Not_Found()
    {
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId).Returns((Sale?)null);

        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(saleId);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        var handler = new DeleteSaleCommandHandler(_repository, _logger);
        var command = new DeleteSaleCommand(Guid.Empty);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}