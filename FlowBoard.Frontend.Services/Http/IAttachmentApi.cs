using Refit;

namespace FlowBoard.Frontend.Services.Http;

public interface IAttachmentApi
{
    [Multipart]
    [Post("/api/attachment/upload")]
    Task<ApiResponse<string>> UploadAsync(StreamPart file);
}
