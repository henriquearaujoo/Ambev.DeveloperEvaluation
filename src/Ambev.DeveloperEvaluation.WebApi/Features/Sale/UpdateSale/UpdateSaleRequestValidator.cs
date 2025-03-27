using Ambev.DeveloperEvaluation.WebApi.Features.Sale.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sale.DeleteSale;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sale.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for DeleteSaleRequest
    /// </summary>
    public UpdateSaleRequestValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required.")
            .MaximumLength(100).WithMessage("Customer must be at most 100 characters.");

        RuleFor(x => x.Branch)
            .NotEmpty().WithMessage("Branch is required.")
            .MaximumLength(100).WithMessage("Branch must be at most 100 characters.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one sale item is required.");

        RuleForEach(x => x.Items).SetValidator(new UpdateSaleItemDtoRequestValidator());
    }
}