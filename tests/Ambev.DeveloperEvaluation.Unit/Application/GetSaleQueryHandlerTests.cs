using Ambev.DeveloperEvaluation.Application.Sale.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleQueryHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "GetSale should return sale when valid request is provided")]
    public async Task GetSale_Should_Return_Sale_When_ValidRequest()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var sale = new Sale { Id = saleId, Customer = _faker.Name.FullName() };
        var expectedResult = new GetSaleResult { Id = saleId };

        _repository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(saleId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
    }

    [Fact(DisplayName = "GetSale should return null when sale not found")]
    public async Task GetSale_Should_Return_Null_When_Sale_Not_Found()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(saleId);

        // Act
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().BeNull();
    }

    [Fact(DisplayName = "GetSale should throw ValidationException when Id is empty")]
    public async Task GetSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        // Arrange
        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(Guid.Empty);

        // Act
        var act = async () => await handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}