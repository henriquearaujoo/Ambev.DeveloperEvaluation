using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sale.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;

public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleService _saleService;
    private readonly ILogger<UpdateSaleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateSaleCommandHandler(ISaleRepository saleRepository, ILogger<UpdateSaleCommandHandler> logger, ISaleService saleService, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _saleService = saleService;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var updatedSale = new Domain.Entities.Sale
        {
            Id = request.Id,
            Customer = request.Customer,
            Branch = request.Branch,
            Items = request.Items.Select(item => new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = item.Product,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        _saleService.ApplyDiscounts(updatedSale);
        updatedSale = await _saleRepository.UpdateAsync(updatedSale);

        _logger.LogInformation("Event: SaleUpdated | SaleId: {SaleId}", updatedSale?.Id);

        return updatedSale != null ? _mapper.Map<UpdateSaleResult>(updatedSale) : null;
    }
}