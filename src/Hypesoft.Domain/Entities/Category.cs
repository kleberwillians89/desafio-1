using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hypesoft.Domain.Entities;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.String)] // Id salvo como string
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? UpdatedAt { get; set; }
}
