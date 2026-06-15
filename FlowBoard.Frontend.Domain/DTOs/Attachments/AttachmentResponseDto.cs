namespace FlowBoard.Frontend.Domain.DTOs.Attachments;

public record AttachmentResponseDto(
    Guid Id,
    string FileName,
    string BlobUrl
);