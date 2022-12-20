using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models;

public class TodoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public long Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }
    
    public string? Secret { get; set; }
}