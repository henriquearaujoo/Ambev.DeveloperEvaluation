using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required.")
            .MaximumLength(100).WithMessage("Customer must be at most 100 characters.");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("Branch is required.")
            .MaximumLength(100).WithMessage("Branch must be at most 100 characters.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemDtoRequestValidator());
    }
}