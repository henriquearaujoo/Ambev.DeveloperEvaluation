using Ambev.DeveloperEvaluation.Application.Sale.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sale.CancelSale;

public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
{
    private readonly ISaleRepository _repository;
    private readonly ILogger<CancelSaleCommandHandler> _logger;

    public CancelSaleCommandHandler(ISaleRepository repository, ILogger<CancelSaleCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _repository.CancelAsync(request.Id, cancellationToken);

        _logger.LogInformation("Event: SaleCancelled | SaleId: {SaleId}", request.Id);

    }
}