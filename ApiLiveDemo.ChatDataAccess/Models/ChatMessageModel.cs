using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiLiveDemo.ChatDataAccess.Models;

public class ChatMessageModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement]
    public string Message { get; set; }
    [BsonElement]
    public string SenderName { get; set; }
    [BsonElement]
    public DateTime? TimeSent { get; set; }
}