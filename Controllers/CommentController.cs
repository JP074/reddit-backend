using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
      private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("post/{postId}/comments")]
        public async Task<ActionResult<ServiceResponse<List<GetCommentResponseDto>>>> Get(int postId) 
        {
            return Ok(await _commentService.GetAllComments(postId));
        }
        [Authorize]
        [HttpPost("post/{postId}/comment")]
        public async Task<ActionResult<ServiceResponse<List<GetCommentResponseDto>>>> AddComment(AddCommentRequestDto newComment, int postId)
        {
            return Ok(await _commentService.AddComment(newComment, postId));
        }
        [Authorize]
        [HttpPut("post/{postId}/comment/{commentId}")]
        public async Task<ActionResult<ServiceResponse<List<GetCommentResponseDto>>>> UpdateComment(int commentId, UpdateCommentRequestDto updatedComment)
        {
            var response  = await _commentService.UpdateComment(commentId, updatedComment);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [Authorize]
        [HttpDelete("post/{postId}/{commentId}")]
        public async Task<ActionResult<ServiceResponse<GetCommentResponseDto>>> DeleteComment(int commentId) 
        {
            var response  = await _commentService.DeleteComment(commentId);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }   
    }
}