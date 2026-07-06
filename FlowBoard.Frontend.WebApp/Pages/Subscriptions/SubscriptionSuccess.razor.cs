using FlowBoard.Frontend.Services.State;
using Microsoft.AspNetCore.Components;


namespace FlowBoard.Frontend.WebApp.Pages.Subscriptions;

public partial class SubscriptionSuccess
{
    [Inject] private UserState UserState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private bool _isPro;

    protected override async Task OnInitializedAsync()
    {
        await PollForProAsync();
    }

    private async Task PollForProAsync()
    {
        for (var attempt = 0; attempt < 6; attempt++)
        {
            await UserState.LoadAsync();

            if (UserState.IsPro)
            {
                _isPro = true;
                StateHasChanged();
                return;
            }

            StateHasChanged();
        }
    }

    private void GoToBoards() 
        => Navigation.NavigateTo("/", forceLoad: true);
}