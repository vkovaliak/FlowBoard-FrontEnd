using Microsoft.AspNetCore.Components;
using MudBlazor;
using FlowBoard.Frontend.Domain.DTOs.Cards;
using FlowBoard.Frontend.Domain.Models.Cards;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Domain.DTOs.Attachments;

namespace FlowBoard.Frontend.WebApp.Components.Dialogs.EditCardDialog;

public partial class EditCardDialog : ComponentBase
{
    [Inject] public IAttachmentService AttachmentService { get; set; } = default!;
    [CascadingParameter] public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public Guid CardId { get; set; }
    [Parameter] public string CurrentName { get; set; } = string.Empty;
    [Parameter] public string? CurrentDescription { get; set; }
    [Parameter] public List<AttachmentResponseDto> Attachments { get; set; } = [];

    private List<AttachmentResponseDto> _attachments = [];
    private CreateCardModel _model = new();

    protected override void OnInitialized()
    {
        _model.Name = CurrentName;
        _model.Description = CurrentDescription;

        _attachments = Attachments.ToList();
    }

    private void HandleNewAttachment(AttachmentResponseDto newAttachment)
    {
        _attachments.Add(newAttachment);
        StateHasChanged();
    }

    private async Task DeleteCardAttachmentAsync(Guid attachmentId)
    {
        var success = await AttachmentService.DeleteCardAttachmentAsync(
            attachmentId);

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
            new UpdateCardDto(_model.Name, _model.Description ?? string.Empty)));
    }
}