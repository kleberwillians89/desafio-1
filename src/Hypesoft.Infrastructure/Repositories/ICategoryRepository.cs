using Hypesoft.Domain.Entities;

namespace Hypesoft.Infrastructure.Repositories;

public interface ICategoryRepository
{
    Task<(IReadOnlyList<Category> Items, long Total)> GetPagedAsync(
        int page, int pageSize, string? name, CancellationToken ct);

    Task<Category?> GetByIdAsync(string id, CancellationToken ct);
    Task<string> CreateAsync(Category category, CancellationToken ct);
    Task<bool> UpdateAsync(Category category, CancellationToken ct);
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
