using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleService _saleService;
    private readonly ILogger<CreateSaleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, 
        ISaleService saleService,
        ILogger<CreateSaleCommandHandler> logger, 
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _saleService = saleService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = _mapper.Map<Domain.Entities.Sale>(request);

        _saleService.ApplyDiscounts(sale);

        sale.SaleNumber = Guid.NewGuid().ToString("N").Substring(0, 8);
        sale.SaleDate = DateTime.UtcNow;

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        _logger.LogInformation("Event: SaleCreated | SaleId: {SaleId}", sale.Id);

        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}