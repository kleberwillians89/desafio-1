using Hypesoft.Domain.Entities;
using Hypesoft.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MongoContext _ctx;
    public ProductRepository(MongoContext ctx) => _ctx = ctx;

    // LISTA PAGINADA
    public async Task<(IReadOnlyList<Product> Items, long Total)> GetPagedAsync(
        int page, int pageSize, string? name, string? categoryId, CancellationToken ct)
    {
        var filter = Builders<Product>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(name))
        {
            // case-insensitive
            filter &= Builders<Product>.Filter.Regex(x => x.Name, new BsonRegularExpression(name, "i"));
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            filter &= Builders<Product>.Filter.Eq(x => x.CategoryId, categoryId);
        }

        var find  = _ctx.Products.Find(filter);
        var total = await find.CountDocumentsAsync(ct);
        var items = await find
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    // BUSCA POR ID
    public async Task<Product?> GetByIdAsync(string id, CancellationToken ct)
        => await _ctx.Products.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    // CRIA
    public async Task CreateAsync(Product product, CancellationToken ct)
    {
        await _ctx.Products.InsertOneAsync(product, cancellationToken: ct);
    }

    // ATUALIZA
    public async Task UpdateAsync(Product product, CancellationToken ct)
    {
        await _ctx.Products.ReplaceOneAsync(
            x => x.Id == product.Id,
            product,
            cancellationToken: ct
        );
    }

    // DELETA
    public async Task DeleteAsync(string id, CancellationToken ct)
    {
        await _ctx.Products.DeleteOneAsync(x => x.Id == id, ct);
    }

    // ATUALIZA ESTOQUE
    public async Task UpdateStockAsync(string id, int newQty, CancellationToken ct)
    {
        var update = Builders<Product>.Update.Set(x => x.StockQty, newQty);
        await _ctx.Products.UpdateOneAsync(x => x.Id == id, update, cancellationToken: ct);
    }

    // ITENS COM BAIXO ESTOQUE
    public async Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct)
    {
        var filter = Builders<Product>.Filter.Lt(x => x.StockQty, threshold);
        return await _ctx.Products.Find(filter).ToListAsync(ct);
    }

    // VALOR TOTAL DO INVENTÁRIO
    public async Task<decimal> GetTotalInventoryValueAsync(CancellationToken ct)
    {
        var all = await _ctx.Products.Find(Builders<Product>.Filter.Empty).ToListAsync(ct);
        return all.Sum(p => p.Price * p.StockQty);
    }

    // CONTAGEM TOTAL
    public async Task<long> GetTotalCountAsync(CancellationToken ct)
    {
        return await _ctx.Products.CountDocumentsAsync(Builders<Product>.Filter.Empty, cancellationToken: ct);
    }

    // CONTAGEM POR CATEGORIA
    public async Task<IReadOnlyList<(string CategoryId, int Count)>> GetCountByCategoryAsync(CancellationToken ct)
    {
        var grouped = await _ctx.Products.Aggregate()
            .Group(p => p.CategoryId, g => new { CategoryId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return grouped.Select(x => (x.CategoryId, (int)x.Count)).ToList();
    }
}
