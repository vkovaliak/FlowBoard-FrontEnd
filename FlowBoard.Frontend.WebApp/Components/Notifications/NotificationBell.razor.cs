using FlowBoard.Frontend.Domain.DTOs.Notifications;
using FlowBoard.Frontend.Domain.Enums;
using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Notifications;

public partial class NotificationBell : IDisposable
{
    [Inject] NotificationState NotificationState { get; set; } = default!;
    [Inject] NavigationManager Navigation { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        NotificationState.OnChange += StateChanged;
        NotificationState.OnOpenChanged += StateChanged;
        await NotificationState.InitializeAsync();
    }

    private void StateChanged() 
        => InvokeAsync(StateHasChanged);

    private void OpenDrawer() 
        => NotificationState.Open();

    private void Close() 
        => NotificationState.Close();

    private void OnDrawerOpenChanged(bool value)
    {
        if (value)
        {
            NotificationState.Open();
        }
        else
        {
            NotificationState.Close();
        }
    }

    private async Task HandleClickAsync(NotificationDto notification)
    {
        if (!notification.IsRead)
        {
            await NotificationState.MarkAsReadAsync(notification.Id);
        }

        NotificationState.Close();

        if (notification.Type == NotificationType.RemovedFromBoard)
        {
            return;
        }

        if (!notification.BoardId.HasValue)
        {
            return;
        }

        var url = notification.CardId.HasValue
            ? $"/boards/{notification.BoardId}?card={notification.CardId}"
            : $"/boards/{notification.BoardId}";

        Navigation.NavigateTo(url);
    }

    private async Task MarkAllReadAsync()
        => await NotificationState.MarkAllAsReadAsync();

    public void Dispose()
    {
        NotificationState.OnChange -= StateChanged;
        NotificationState.OnOpenChanged -= StateChanged;
    }
}