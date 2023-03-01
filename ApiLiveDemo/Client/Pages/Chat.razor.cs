using System.Net.Http.Json;
using ApiLiveDemo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ApiLiveDemo.Client.Pages;

partial class Chat : ComponentBase
{
    public ChatMessageDto CurrentMessage { get; set; } = new();
    public List<ChatMessageDto> Messages { get; set; }

    private string UserName;

    private HubConnection _chatHub;

    protected override async Task OnInitializedAsync()
    {
        var userInfo = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        UserName = userInfo.User.Identity.Name;
        Messages = new List<ChatMessageDto>();

        _chatHub = new HubConnectionBuilder()
            .WithUrl(NavigationManager.BaseUri + "hubs/chat")
            .Build();

        _chatHub.On<ChatMessageDto>("BroadcastMessage", (message) =>
        {
            Messages.Add(message);
            StateHasChanged();
        });

        var response = await HttpClient.GetFromJsonAsync<ChatMessageDto[]>("allMessages");

        if (response != null)
        {
            Messages.AddRange(response);
        }

        await _chatHub.StartAsync();
        await base.OnInitializedAsync();
    }

    private async Task SendMessage()
    {
        CurrentMessage.Name = UserName;
        CurrentMessage.Timestamp = DateTime.UtcNow;
        await _chatHub.SendAsync("BroadcastMessage", CurrentMessage);
        CurrentMessage.Message = string.Empty;
    }
}