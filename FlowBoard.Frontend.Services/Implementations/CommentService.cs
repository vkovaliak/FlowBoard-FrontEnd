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

    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(Guid boardId, Guid cardId)
    {
        var response = await _commentApi.GetByCardIdAsync(boardId, cardId);

        return response.IsSuccessStatusCode
            && response.Content != null ? response.Content : [];
    }

    public async Task<Guid> CreateAsync(Guid boardId, Guid cardId, CreateCommentDto comment)
    {
        var response = await _commentApi.CreateAsync(boardId, cardId, comment);

        return response.Content;
    }

    public async Task<bool> UpdateAsync(Guid boardId, Guid cardId, Guid commentId, UpdateCommentDto comment)
    {
        var response = await _commentApi.UpdateAsync(boardId, cardId, commentId, comment);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid cardId, Guid commentId)
    {
        var response = await _commentApi.DeleteAsync(boardId, cardId, commentId);

        return response.IsSuccessStatusCode
            && response.Content != false;
    }
}