using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Services.CommentService
{
    public interface ICommentService
    {
        Task<ServiceResponse<List<GetCommentResponseDto>>> GetAllComments(int postId);
        Task<ServiceResponse<List<GetCommentResponseDto>>> AddComment(AddCommentRequestDto newComment, int postId);
        Task<ServiceResponse<GetCommentResponseDto>> UpdateComment(int commentId,UpdateCommentRequestDto updatedComment);
        Task<ServiceResponse<List<GetCommentResponseDto>>> DeleteComment(int commentId);
    }
}