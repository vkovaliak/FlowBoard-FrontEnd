using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class CommentService : ICommentService
{
    public readonly ICommentApi _commentApi;

    public CommentService(ICommentApi commentApi)
    {
        _commentApi = commentApi;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid cardId)
    {
        var response = await _commentApi.GetByCardIdAsync(cardId);

        return response.IsSuccessStatusCode 
            && response.Content != null ? response.Content : [];
    }

    public async Task<bool> CreateAsync(Guid cardId, CreateCommentDto comment)
    {
        var response = await _commentApi.CreateAsync(cardId, comment);

        return response.IsSuccessStatusCode 
            && response.Content != Guid.Empty;
    }
}