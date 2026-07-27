using FlowBoard.Frontend.Domain.DTOs.Notifications;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Notifications;

public partial class NotificationBell : IDisposable
{
    [Inject] NotificationState NotificationState { get; set; } = default!;
    [Inject] NavigationManager Navigation { get; set; } = default!;

    private bool _isOpen;

    protected override async Task OnInitializedAsync()
    {
        NotificationState.OnChange += StateChanged;
        await NotificationState.InitializeAsync();
    }

    private void StateChanged() 
        => InvokeAsync(StateHasChanged);

    private void OpenDrawer() => _isOpen = true;

    private void Close() => _isOpen = false;

    private void OnDrawerOpenChanged(bool value) 
        => _isOpen = value;

    private async Task HandleClickAsync(NotificationDto notification)
    {
        if (!notification.IsRead)
        {
            await NotificationState.MarkAsReadAsync(notification.Id);
        }

        _isOpen = false;

        if (notification.BoardId.HasValue)
        {
            Navigation.NavigateTo($"/boards/{notification.BoardId}");
        }
    }

    private async Task MarkAllReadAsync()
        => await NotificationState.MarkAllAsReadAsync();

    public void Dispose()
    {
        NotificationState.OnChange -= StateChanged;
    }
}