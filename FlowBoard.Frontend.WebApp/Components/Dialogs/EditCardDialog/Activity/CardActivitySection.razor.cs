using System.Globalization;
using FlowBoard.Frontend.Domain.DTOs.Activities;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog.Activity;

public partial class CardActivitySection : IAsyncDisposable
{
    [Inject] public ICardService CardService { get; set; } = default!;
    [Inject] public IBoardHubService BoardHub { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }

    private List<ActivityDto> _activities = new();
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        BoardHub.OnBoardUpdated += HandleBoardUpdated;
        await LoadActivitiesAsync();
    }

    private async void HandleBoardUpdated(Guid updatedBoardId)
    {
        if (updatedBoardId != BoardId)
        {
            return;
        }

        await LoadActivitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadActivitiesAsync()
    {
        _isLoading = true;
        _activities = await CardService.GetCardActivitiesAsync(
            BoardId, CardId);
        _isLoading = false;
    }

    private string FormatDate(DateTime date)
        => date.ToLocalTime().ToString(
            "dd MMM yyyy, HH:mm", CultureInfo.InvariantCulture);

    public ValueTask DisposeAsync()
    {
        BoardHub.OnBoardUpdated -= HandleBoardUpdated;
        return ValueTask.CompletedTask;
    }
}