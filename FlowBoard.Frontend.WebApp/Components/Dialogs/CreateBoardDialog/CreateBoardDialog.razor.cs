using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.Models.Boards;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Services.Abstractions;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.CreateBoardDialog;

public partial class CreateBoardDialog : ComponentBase
{
    [CascadingParameter] 
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] 
    public BoardDto? CurrentBoard { get; set; }

    [Inject] private IBoardService BoardService { get; set; } = default!;

    public CreateBoardModel Model { get; set; } = new();

    public bool IsEditMode => CurrentBoard != null;

    private List<BoardBackgroundDto> _backgrounds = [];
    private bool _isLoadingBackgrounds = true;

    protected override async Task OnInitializedAsync()
    {
        if (IsEditMode && CurrentBoard != null)
        {
            Model.Name = CurrentBoard.Name;
            Model.IsPublic = CurrentBoard.IsPublic;
            Model.Background = string.IsNullOrEmpty(CurrentBoard.Background)
                ? null
                : CurrentBoard.Background;
        }

        _backgrounds = await BoardService.GetBackgroundsAsync();
        _isLoadingBackgrounds = false;
    }

    private void SelectBackground(string? url)
    {
        Model.Background = Model.Background == url ? null : url;
    }

    private bool IsSelected(string? url) => Model.Background == url;

    public void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }

    public void Cancel()
    {
        MudDialog.Cancel();
    }
}