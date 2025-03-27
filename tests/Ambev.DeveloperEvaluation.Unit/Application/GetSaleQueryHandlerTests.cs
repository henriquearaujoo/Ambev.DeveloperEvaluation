using Ambev.DeveloperEvaluation.Application.Sale.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class GetSaleQueryHandlerTests
{
    private readonly ISaleRepository _repository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly Faker _faker = new();

    [Fact]
    public async Task GetSale_Should_Return_Sale_When_ValidRequest()
    {
        var saleId = Guid.NewGuid();
        var sale = new Sale { Id = saleId, Customer = _faker.Name.FullName() };
        var expectedResult = new GetSaleResult { Id = saleId };

        _repository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(saleId);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(expectedResult.Id, result.Id);
    }

    [Fact]
    public async Task GetSale_Should_Return_Null_When_Sale_Not_Found()
    {
        var saleId = Guid.NewGuid();
        _repository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(saleId);
        var response = await handler.Handle(query, CancellationToken.None);
        Assert.Null(response);
    }

    [Fact]
    public async Task GetSale_Should_Throw_ValidationException_When_Id_Is_Empty()
    {
        var handler = new GetSaleQueryHandler(_repository, _mapper);
        var query = new GetSaleQuery(Guid.Empty);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(query, CancellationToken.None));
    }
}