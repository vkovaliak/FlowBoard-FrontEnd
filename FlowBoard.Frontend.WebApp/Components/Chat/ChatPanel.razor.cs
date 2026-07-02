using FlowBoard.Frontend.Domain.DTOs.Chat;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Chat;

public partial class ChatPanel
{
    [Inject] public IChatService ChatService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;

    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    private readonly List<ChatMessage> _messages = new();
    private string _input = string.Empty;
    private bool _isLoading;
    private bool _shouldScroll;


    private async Task Close()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
    }

    protected override async Task OnAfterRenderAsync(
        bool firstRender)
    {
        if (_shouldScroll)
        {
            _shouldScroll = false;
            await JS.InvokeVoidAsync(
                "scrollChatToBottom", "chat-messages");
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !e.ShiftKey)
        {
            await SendAsync();
        }
    }

    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(_input) || _isLoading)
        {
            return;
        }

        var userText = _input.Trim();
        _messages.Add(new ChatMessage(userText, IsUser: true));
        _input = string.Empty;
        _shouldScroll = true; 
        _isLoading = true;
        StateHasChanged(); 

        var result = await ChatService.SendMessageAsync(
            new ChatRequest(userText));

        _isLoading = false;

        if (result.Success && result.Value is not null)
        {
            _messages.Add(new ChatMessage(
                result.Value.Answer, IsUser: false));
            _shouldScroll = true;
        }
        else
        {
            _messages.Add(new ChatMessage(
                "Sorry, something went wrong. Please try again.", 
                IsUser: false));
        }

        await ScrollToBottom();
    }

    private async Task ScrollToBottom()
    {
        await Task.Yield();
        await JS.InvokeVoidAsync(
            "scrollChatToBottom", "chat-messages");
    }
}
