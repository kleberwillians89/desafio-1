using Hypesoft.Domain.Entities;

namespace Hypesoft.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, long Total)> GetPagedAsync(
        int page, int pageSize, string? name, string? categoryId, CancellationToken ct);

    Task<Product?> GetByIdAsync(string id, CancellationToken ct);

    Task CreateAsync(Product product, CancellationToken ct);          // <- Task
    Task UpdateAsync(Product product, CancellationToken ct);          // <- Task
    Task DeleteAsync(string id, CancellationToken ct);                // <- Task
    Task UpdateStockAsync(string id, int newQty, CancellationToken ct); // <- Task

    Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct);
    Task<decimal> GetTotalInventoryValueAsync(CancellationToken ct);
    Task<long> GetTotalCountAsync(CancellationToken ct);
    Task<IReadOnlyList<(string CategoryId, int Count)>> GetCountByCategoryAsync(CancellationToken ct);
}

