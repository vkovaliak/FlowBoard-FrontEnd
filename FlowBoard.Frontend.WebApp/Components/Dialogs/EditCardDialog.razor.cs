using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs;

public partial class EditCardDialog : ComponentBase
{
    [CascadingParameter] 
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject] 
    public ICommentService CommentService { get; set; } = default!;

    [Parameter] 
    public Guid CardId { get; set; }

    [Parameter] 
    public string CurrentName { get; set; } = string.Empty;

    [Parameter] 
    public string? CurrentDescription { get; set; }

    private CreateCardModel _model = new();

    private IEnumerable<CommentDto> _comments = [];
    private string _newCommentMessage = string.Empty;
    private bool _isLoadingComments = false;

    protected override async Task OnInitializedAsync()
    {
        _model.Name = CurrentName;
        _model.Description = CurrentDescription;

        await LoadCommentsAsync();
    }

    private void Cancel() 
        => MudDialog.Cancel();

    private void SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_model.Name))
        {
            return;
        }
        
        MudDialog.Close(DialogResult.Ok(
            new UpdateCardDto(_model.Name, _model.Description!)));
    }

    private async Task LoadCommentsAsync()
    {
        _isLoadingComments = true;
        _comments = await CommentService.GetCommentsAsync(CardId);
        _isLoadingComments = false;
    }

    private async Task SendCommentAsync()
    {
        if (string.IsNullOrWhiteSpace(_newCommentMessage))
        {
            return;
        }

        var dto = new CreateCommentDto(_newCommentMessage);
        var success = await CommentService.CreateAsync(CardId, dto);

        if (success)
        {
            _newCommentMessage = string.Empty;
            await LoadCommentsAsync();
        }
    }
}