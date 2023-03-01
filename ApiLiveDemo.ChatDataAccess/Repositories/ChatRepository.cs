using ApiLiveDemo.ChatDataAccess.Models;
using ApiLiveDemo.Shared;
using MongoDB.Driver;

namespace ApiLiveDemo.ChatDataAccess.Repositories;

public class ChatRepository
{
    private readonly IMongoCollection<ChatMessageModel> _chatMessages;

    public ChatRepository()
    {
        var settings = MongoClientSettings.FromConnectionString("mongodb+srv://niklas:Apa123@cluster0.j0lyf.mongodb.net/?retryWrites=true&w=majority");
        var client = new MongoClient(settings);
        var database = client.GetDatabase("Chat");
        _chatMessages =
            database.GetCollection<ChatMessageModel>("AllChat",
                new MongoCollectionSettings() {AssignIdOnInsert = true});
    }

    public async Task AddAsync(ChatMessageDto dto)
    {
        await _chatMessages.InsertOneAsync(ConvertToModel(dto));
    }

    public async Task<ChatMessageDto[]> GetAllMessages()
    {
        var filter = Builders<ChatMessageModel>.Filter.Empty;
        var all = await _chatMessages.FindAsync(filter);

        return all.ToList().Select(ConvertToDto).ToArray();
    }

    private ChatMessageModel ConvertToModel(ChatMessageDto dto)
    {
        return new ChatMessageModel()
        {
            SenderName = dto.Name,
            Message = dto.Message,
            TimeSent = dto.Timestamp
        };
    }

    private ChatMessageDto ConvertToDto(ChatMessageModel dataModel)
    {
        return new ChatMessageDto()
        {
            Name = dataModel.SenderName,
            Message = dataModel.Message,
            Timestamp = dataModel.TimeSent
        };
    }
}