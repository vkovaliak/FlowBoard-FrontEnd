using FlowBoard.Frontend.Domain.DTOs.Comments;
using FlowBoard.Frontend.Domain.Models.Common;
using FlowBoard.Frontend.Services.Abstractions;
using FlowBoard.Frontend.Services.Helpers;
using FlowBoard.Frontend.Services.Http;

namespace FlowBoard.Frontend.Services.Implementations;

public class CommentService : ICommentService
{
    public readonly ICommentApi _commentApi;

    public CommentService(ICommentApi commentApi)
    {
        _commentApi = commentApi;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(
        Guid boardId, Guid cardId)
    {
        var response = await _commentApi.GetByCardIdAsync(boardId, cardId);

        return response.IsSuccessStatusCode
            && response.Content != null ? response.Content : [];
    }

    public async Task<OperationResult<Guid>> CreateAsync(
        Guid boardId, Guid cardId, CreateCommentDto comment)
    {
        var response = await _commentApi.CreateAsync(boardId, cardId, comment);

        if (response.IsSuccessStatusCode 
            && response.Content is not null
            && response.Content.CommentId != Guid.Empty)
        {
            return OperationResult<Guid>.Ok(response.Content.CommentId);
        }

        return OperationResult<Guid>.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> UpdateAsync(
        Guid boardId, Guid cardId, Guid commentId, UpdateCommentDto comment)
    {
        var response = await _commentApi.UpdateAsync(
            boardId, cardId, commentId, comment);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }

    public async Task<OperationResult> DeleteAsync(
        Guid boardId, Guid cardId, Guid commentId)
    {
        var response = await _commentApi.DeleteAsync(
            boardId, cardId, commentId);

        if (response.IsSuccessStatusCode)
        {
            return OperationResult.Ok();
        }

        return OperationResult.Fail(response.GetErrorMessage());
    }
}