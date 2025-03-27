using Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
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

    [Fact]
    public async Task UpdateSale_Should_Succeed_When_ValidRequestWithDiscount()
    {
        var updatedSale = new Sale();

        var command = new UpdateSaleCommand
        {
            Id = updatedSale.Id,
            Customer = _faker.Company.CompanyName(),
            Branch = _faker.Address.City(),
            Items = new List<UpdateSaleItemDto>
            {
                new() { Product = "Beer", Quantity = 10, UnitPrice = 5.0m },
                new() { Product = "Beer", Quantity = 5, UnitPrice = 5.0m }
            }
        };

        _repository.UpdateAsync(Arg.Any<Sale>()).Returns(updatedSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult { Id = command.Id });

        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.Id, result.Id);
        await _repository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.Customer == command.Customer && s.Items.Count == 2));
        _saleService.Received(1).ApplyDiscounts(Arg.Any<Sale>());
    }

    [Fact]
    public async Task UpdateSale_Should_Throw_When_Quantity_Exceeds_Maximum()
    {
        var command = new UpdateSaleCommand
        {
            Id = Guid.NewGuid(),
            Customer = _faker.Name.FullName(),
            Branch = _faker.Address.City(),
            Items = new List<UpdateSaleItemDto>
            {
                new() { Product = "Juice", Quantity = 25, UnitPrice = 3.0m }
            }
        };

        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateSale_Should_Throw_ValidationException_When_Invalid_Command()
    {
        var command = new UpdateSaleCommand();
        var handler = new UpdateSaleCommandHandler(_repository, _logger, _saleService, _mapper);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }
}