using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.Domain.DTOs.Boards;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class CreateBoardDialog : ComponentBase
{
    [CascadingParameter] 
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] 
    public BoardDto? CurrentBoard { get; set; }

    public CreateBoardModel Model { get; set; } = new();

    public bool IsEditMode => CurrentBoard != null;

    protected override void OnInitialized()
    {
        if (IsEditMode && CurrentBoard != null)
        {
            Model.Name = CurrentBoard.Name;
            Model.IsPublic = CurrentBoard.IsPublic;
        }
    }

    public void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    public void Cancel()
    {
        MudDialog.Cancel();
    }
}