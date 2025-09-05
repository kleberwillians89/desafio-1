using Hypesoft.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.Infrastructure.Data;

public class MongoContext
{
    public IMongoDatabase Database { get; }
    public IMongoCollection<Product> Products => Database.GetCollection<Product>("products");
    public IMongoCollection<Category> Categories => Database.GetCollection<Category>("categories");

    private readonly IMongoClient _client;

    public MongoContext(string connectionString, string database)
    {
        _client = new MongoClient(connectionString);
        Database = _client.GetDatabase(database);
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        // Índices úteis
        var prodKeys = Builders<Product>.IndexKeys
            .Ascending(p => p.Name)
            .Ascending(p => p.CategoryId)
            .Ascending(p => p.StockQty);   // <<< era Stock

        Products.Indexes.CreateOne(new CreateIndexModel<Product>(prodKeys));

        var catKeys = Builders<Category>.IndexKeys
            .Ascending(c => c.Name);

        Categories.Indexes.CreateOne(new CreateIndexModel<Category>(catKeys));
    }
}
