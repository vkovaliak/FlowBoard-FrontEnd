using FlowBoard.Frontend.Services.Handlers;
using Microsoft.AspNetCore.Components;

namespace FlowBoard.Frontend.WebApp.Components.Subscription;

public partial class UpgradeCard
{
    [Inject] private UpgradeHandler UpgradeHandler { get; set; } = default!;

    private bool _loading;

    private async Task Upgrade()
    {
        _loading = true;
        await UpgradeHandler.StartUpgradeAsync();
    }
}