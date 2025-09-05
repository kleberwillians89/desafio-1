using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Domain.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string CategoryId { get; set; } = default!;
    public int StockQty { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
}
