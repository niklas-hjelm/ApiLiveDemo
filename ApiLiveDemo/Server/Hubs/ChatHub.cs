﻿using ApiLiveDemo.ChatDataAccess.Repositories;
using ApiLiveDemo.Shared;
using Microsoft.AspNetCore.SignalR;

namespace ApiLiveDemo.Server.Hubs;

public class ChatHub : Hub
{
    private readonly ChatRepository _chatRepository;

    public ChatHub(ChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task BroadcastMessage(ChatMessageDto message)
    {
        await _chatRepository.AddAsync(message);
        await Clients.All.SendAsync("BroadcastMessage", message);
    }
}