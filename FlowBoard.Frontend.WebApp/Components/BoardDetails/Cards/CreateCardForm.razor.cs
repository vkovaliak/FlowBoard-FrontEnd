using Microsoft.AspNetCore.Components;
using FlowBoard.Frontend.Domain.Models.Cards;

namespace FlowBoard.Frontend.WebApp.Components.BoardDetails.Cards;

public partial class CreateCardForm
{
    [Parameter] public EventCallback<string> OnCreate { get; set; }
    [Parameter] public bool CanEdit { get; set; } = true;

    private bool _isCreating;
    private CreateCardModel _form = new();

    public void Open()
    {
        if (!CanEdit)
        {
            return;
        }
        
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
        _form.Name = string.Empty;
    }
}