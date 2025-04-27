using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task<Sale?> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingSale = await _context.Sales.FirstOrDefaultAsync(
            s => s.Id == sale.Id, cancellationToken);
        if (existingSale == null)
        {
            return null;
        }

        existingSale.Customer = sale.Customer;
        existingSale.Branch = sale.Branch;
        existingSale.UpdatedAt = DateTime.UtcNow;

        var itemsToRemove = await _context.SaleItems
            .Where(i => i.SaleId == sale.Id)
            .ToListAsync(cancellationToken);

        _context.SaleItems.RemoveRange(itemsToRemove);

        foreach (var item in sale.Items)
        {
            item.Id = Guid.NewGuid();
            item.SaleId = sale.Id;
            _context.SaleItems.Add(item);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return existingSale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingSale = await _context.Sales.FirstOrDefaultAsync(
            s => s.Id == id, cancellationToken);
        if (existingSale == null)
        {
            throw new KeyNotFoundException($"Sale with Id {id} not found.");
        }
        
        existingSale.Cancel();

        await _context.SaveChangesAsync(cancellationToken);
    }
}