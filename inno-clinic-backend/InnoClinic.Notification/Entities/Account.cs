using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoClinic.Notification.Entities;

public class Account
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("AccountId")]
    public Guid Id { get; set; }

    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonElement("TelegramId")]
    public string? TelegramId { get; set; }
}