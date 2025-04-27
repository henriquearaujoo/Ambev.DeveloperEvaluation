﻿using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.CancelSale;

public class CancelSaleCommand : IRequest
{
    public Guid Id { get; }

    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}