using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Models.Boards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class CreateBoardDialog : ComponentBase
{
    [CascadingParameter] 
    public IMudDialogInstance MudDialog { get; set; } = default!;

    public CreateBoardModel Model { get; set; } = new();

    public void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    public void Cancel()
    {
        MudDialog.Cancel();
    }
}