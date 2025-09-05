using Hypesoft.Domain.Entities;
using Hypesoft.Infrastructure.Data;
using MongoDB.Driver;
using MongoDB.Bson; 


namespace Hypesoft.Infrastructure.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly MongoContext _ctx;

        public CategoryRepository(MongoContext ctx) => _ctx = ctx;

        public async Task<(IReadOnlyList<Category> Items, long Total)> GetPagedAsync(
            int page, int pageSize, string? name, CancellationToken ct)
        {
            var filter = string.IsNullOrWhiteSpace(name)
                ? Builders<Category>.Filter.Empty
                : Builders<Category>.Filter.Regex(
                    x => x.Name,
                    new BsonRegularExpression(name, "i")
                  );

            var find = _ctx.Categories.Find(filter);
            var total = await find.CountDocumentsAsync(ct);
            var items = await find.Skip((page - 1) * pageSize)
                                  .Limit(pageSize)
                                  .ToListAsync(ct);

            return (items, total);
        }

        // <-- CORRIGIDO: só esse GetByIdAsync existe agora
        public async Task<Category?> GetByIdAsync(string id, CancellationToken ct)
        {
            return await _ctx.Categories
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync(ct); // pode retornar null, por isso Category?
        }

        public async Task<string> CreateAsync(Category category, CancellationToken ct)
        {
            category.Id = Guid.NewGuid().ToString("N");
            category.CreatedAt = DateTime.UtcNow;
            await _ctx.Categories.InsertOneAsync(category, cancellationToken: ct);
            return category.Id;
        }

        public async Task<bool> UpdateAsync(Category category, CancellationToken ct)
        {
            category.UpdatedAt = DateTime.UtcNow;
            var result = await _ctx.Categories.ReplaceOneAsync(
                x => x.Id == category.Id,
                category,
                new ReplaceOptions { IsUpsert = false },
                ct);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken ct)
        {
            var result = await _ctx.Categories.DeleteOneAsync(x => x.Id == id, ct);
            return result.DeletedCount > 0;
        }
    }
}
