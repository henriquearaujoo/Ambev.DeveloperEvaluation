using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command captures the required information to register a new sale, including
/// the customer, branch, and a list of items sold. It implements <see cref="IRequest{TResponse}"/>
/// to initiate a request that returns the newly created sale's ID.
/// 
/// The data is validated using the <see cref="CreateSaleCommandValidator"/> to ensure
/// that the required fields are properly populated and follow the business rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the customer who made the purchase.
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch where the sale occurred.
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of items included in the sale.
    /// </summary>
    public List<CreateSaleItemDto> Items { get; set; } = new();

}