using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.Models.Cards;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CreateCardForm
{
    [Parameter] public EventCallback<string> OnCreate { get; set; }

    private bool _isCreating;
    private CreateCardModel _form = new();

    public void Open()
    {
        _isCreating = true;
        _form = new CreateCardModel();
        StateHasChanged();
    }

    private void Close() => _isCreating = false;

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(_form.Name))
        { 
            return;
        }
        
        await OnCreate.InvokeAsync(_form.Name);
        Close();
    }
}