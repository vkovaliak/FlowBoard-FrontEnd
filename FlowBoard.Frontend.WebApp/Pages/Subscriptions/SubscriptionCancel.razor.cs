using FlowBoard.Frontend.Services.Handlers;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Pages.Subscriptions;

public partial class SubscriptionCancel
{
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private bool _loading;

    private void GoToBoards() 
        => Navigation.NavigateTo("/");

    private async Task TryAgain()
    {
        _loading = true;
        await UpgradeHandler.StartUpgradeAsync();
    }
}