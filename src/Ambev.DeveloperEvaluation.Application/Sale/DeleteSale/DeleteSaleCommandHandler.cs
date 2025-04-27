using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sale.DeleteSale;

public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<DeleteSaleCommandHandler> _logger;

    public DeleteSaleCommandHandler(ISaleRepository saleRepository, ILogger<DeleteSaleCommandHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found.");

        await _saleRepository.DeleteAsync(request.Id, cancellationToken);

        _logger.LogInformation("Event: SaleDeleted | SaleId: {SaleId}", request.Id);
    }
}