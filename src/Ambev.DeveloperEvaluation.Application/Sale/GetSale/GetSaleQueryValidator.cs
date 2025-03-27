using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale;

public class GetSaleQueryValidator : AbstractValidator<GetSaleQuery>
{
    public GetSaleQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
}