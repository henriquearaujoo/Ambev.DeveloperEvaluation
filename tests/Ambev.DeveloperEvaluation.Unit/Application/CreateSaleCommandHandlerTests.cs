using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleCommandHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ILogger<CreateSaleCommandHandler> _logger = Substitute.For<ILogger<CreateSaleCommandHandler>>();
    private readonly ISaleService _saleService = Substitute.For<ISaleService>();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "CreateSale should succeed when valid request with discount")]
    public async Task CreateSale_Should_Succeed_When_ValidRequestWithDiscount()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Customer = _faker.Company.CompanyName(),
            Branch = _faker.Address.City(),
            Items = new List<CreateSaleItemDto>
            {
                new() { Product = "Produto 1", Quantity = 10, UnitPrice = 5.0m },
                new() { Product = "Produto 1", Quantity = 5, UnitPrice = 5.0m }
            }
        };

        var sale = new Sale
        {
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new() { Product = "Produto 1", Quantity = 10, UnitPrice = 5.0m },
                new() { Product = "Produto 1", Quantity = 5, UnitPrice = 5.0m }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _repository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });

        var handler = new CreateSaleCommandHandler(_repository, _saleService, _logger, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "CreateSale should succeed when no discount applies")]
    public async Task CreateSale_Should_Succeed_When_NoDiscountApplies()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Customer = _faker.Name.FullName(),
            Branch = _faker.Address.City(),
            Items = new List<CreateSaleItemDto>
            {
                new() { Product = "Produto 2", Quantity = 2, UnitPrice = 2.0m }
            }
        };

        var sale = new Sale
        {
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new() { Product = "Produto 2", Quantity = 2, UnitPrice = 2.0m }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _repository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });

        var handler = new CreateSaleCommandHandler(_repository, _saleService, _logger, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "CreateSale should throw ValidationException when quantity exceeds maximum allowed")]
    public async Task CreateSale_Should_Throw_When_Quantity_Exceeds_Maximum()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Customer = _faker.Name.FullName(),
            Branch = _faker.Address.City(),
            Items = new List<CreateSaleItemDto>
            {
                new() { Product = "Produto 3", Quantity = 25, UnitPrice = 3.0m }
            }
        };

        var sale = new Sale
        {
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new() { Product = "Produto 3", Quantity = 25, UnitPrice = 3.0m }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);

        var handler = new CreateSaleCommandHandler(_repository, _saleService, _logger, _mapper);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "CreateSale should handle multiple different products correctly")]
    public async Task CreateSale_Should_Handle_Multiple_Different_Products()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Customer = _faker.Name.FullName(),
            Branch = _faker.Address.City(),
            Items = new List<CreateSaleItemDto>
            {
                new() { Product = "Produto 1", Quantity = 5, UnitPrice = 5.0m },
                new() { Product = "Produto 2", Quantity = 5, UnitPrice = 3.0m }
            }
        };

        var sale = new Sale
        {
            Customer = command.Customer,
            Branch = command.Branch,
            Items = new List<SaleItem>
            {
                new() { Product = "Produto 1", Quantity = 5, UnitPrice = 5.0m },
                new() { Product = "Produto 2", Quantity = 5, UnitPrice = 3.0m }
            }
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _repository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(new CreateSaleResult { Id = sale.Id });

        var handler = new CreateSaleCommandHandler(_repository, _saleService, _logger, _mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
    }

    [Fact(DisplayName = "CreateSale should throw ValidationException when command is invalid")]
    public async Task CreateSale_Should_Throw_ValidationException_When_Invalid_Command()
    {
        // Arrange
        var command = new CreateSaleCommand();
        var handler = new CreateSaleCommandHandler(_repository, _saleService, _logger, _mapper);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}