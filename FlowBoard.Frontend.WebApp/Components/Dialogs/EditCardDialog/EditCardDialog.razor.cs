using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Attachments;
using FlowBoard.Frontend.Domain.DTOs.Boards;
using FlowBoard.Frontend.Domain.DTOs.Labels;
using FlowBoard.Frontend.Domain.DTOs.Lists;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

public partial class EditCardDialog : ComponentBase
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid BoardId { get; set; }
    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string CurrentName { get; set; } = string.Empty;
    [Parameter] public string? CurrentDescription { get; set; }
    [Parameter] public List<AttachmentResponseDto> Attachments { get; set; } = [];
    [Parameter] public List<CardAssigneeDto> Assignees { get; set; } = [];
    [Parameter] public List<BoardMemberDto> BoardMembers { get; set; } = [];
    [Parameter] public DateTime? CurrentDueDate { get; set; }
    [Parameter] public bool IsCompleted { get; set; }
    [Parameter] public List<LabelDto> BoardLabels { get; set; } = [];
    [Parameter] public List<LabelDto> AttachedLabels { get; set; } = [];
    [Parameter] public List<LabelDto> CardLabels { get; set; } = [];
    [Parameter] public Guid CurrentListId { get; set; }
    [Parameter] public List<ListDto> Lists { get; set; } = [];

    private List<AttachmentResponseDto> _attachments = [];
    private List<LabelDto> _attachedLabels = [];
    private CreateCardModel _model = new();
    private bool _isCompleted;

    protected override void OnInitialized()
    {
        _model.Name = CurrentName;
        _model.Description = CurrentDescription;
        _model.DueDate = CurrentDueDate;
        _isCompleted = IsCompleted;
        _attachments = Attachments.ToList();
        _attachedLabels = AttachedLabels.ToList();
    }

    private void HandleNewAttachment(AttachmentResponseDto newAttachment)
    {
        _attachments.Add(newAttachment);
        StateHasChanged();
    }

    private async Task DeleteCardAttachmentAsync(Guid attachmentId)
    {
        var success = await AttachmentService.DeleteCardAttachmentAsync(
            BoardId, CardId, attachmentId);

        if (success)
        {
            _attachments.RemoveAll(a => a.Id == attachmentId);
            StateHasChanged();
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(_model.Name))
        {
            return;
        }

        MudDialog.Close(DialogResult.Ok(
            new UpdateCardDto(
                _model.Name, 
                _model.Description ?? string.Empty,
                _model.DueDate)));
    }

    private async Task OnLabelsChanged()
    {
        await Task.CompletedTask;
    }

    private async Task OnCardMoved()
    {
        await Task.CompletedTask;
    }
}