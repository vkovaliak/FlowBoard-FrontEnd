using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.Services.Handlers;

public class UpgradeHandler
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly NavigationManager _navigation;
    private readonly ISnackbar _snackbar;

    public UpgradeHandler(
        ISubscriptionService subscriptionService,
        NavigationManager navigation,
        ISnackbar snackbar)
    {
        _subscriptionService = subscriptionService;
        _navigation = navigation;
        _snackbar = snackbar;
    }

    public async Task StartUpgradeAsync()
    {
        var url = await _subscriptionService.CreateCheckoutAsync();

        if (string.IsNullOrEmpty(url))
        {
            _snackbar.Add(
                "Failed to start checkout. Please try again.", Severity.Error);
            return;
        }

        _navigation.NavigateTo(url, forceLoad: true);
    }
}