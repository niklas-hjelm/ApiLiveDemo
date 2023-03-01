
using ApiLiveDemo.ChatDataAccess.Repositories;
using ApiLiveDemo.Server.Hubs;

namespace ApiLiveDemo.Server.Extensions;

public static class WebApplicationEndpointExtensions
{
    public static WebApplication MapChatEndpoints(this WebApplication app)
    {
        app.MapHub<ChatHub>("/hubs/chat");
        app.MapGet("/allMessages", async (ChatRepository repo) => await repo.GetAllMessages());

        return app;
    }

}