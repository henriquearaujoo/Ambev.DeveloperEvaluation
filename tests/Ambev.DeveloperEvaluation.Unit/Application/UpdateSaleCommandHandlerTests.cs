using Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class UpdateSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<UpdateSaleCommandHandler> _logger = Substitute.For<ILogger<UpdateSaleCommandHandler>>();
    private readonly ISaleService _saleService = Substitute.For<ISaleService>();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "UpdateSale should succeed with valid request and apply discounts")]
    public async Task UpdateSale_Should_Succeed_When_ValidRequestWithDiscount()
    {
        // Arrange
        var updatedSale = new Sale();
        var command = new UpdateSaleCommand
        {
            Id = updatedSale.Id,
            Customer = _faker.Company.CompanyName(),
            Branch = _faker.Address.City(),
            Items = new List<UpdateSaleItemDto>
            {
                new() { Product = "produto 1", Quantity = 10, UnitPrice = 5.0m },
                new() { Product = "produto 1", Quantity = 5, UnitPrice = 5.0m }
            }
        };

        _repository.UpdateAsync(Arg.Any<Sale>()).Returns(updatedSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult { Id = command.Id });

        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(command.Id);
        await _repository.Received(1).UpdateAsync(Arg.Is<Sale>(s =>
            s.Customer == command.Customer &&
            s.Items.Count == 2));

        _saleService.Received(1).ApplyDiscounts(Arg.Any<Sale>());
    }

    [Fact(DisplayName = "UpdateSale should throw validation exception when quantity exceeds maximum")]
    public async Task UpdateSale_Should_Throw_When_Quantity_Exceeds_Maximum()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            Customer = _faker.Name.FullName(),
            Branch = _faker.Address.City(),
            Items = new List<UpdateSaleItemDto>
            {
                new() { Product = "Produto 2", Quantity = 25, UnitPrice = 3.0m }
            }
        };

        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "UpdateSale should throw validation exception when command is invalid")]
    public async Task UpdateSale_Should_Throw_ValidationException_When_Invalid_Command()
    {
        // Arrange
        var command = new UpdateSaleCommand();
        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}