using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace reddit_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("posts")]
        public async Task<ActionResult<ServiceResponse<List<GetPostResponseDto>>>> Get()
        {
            return Ok(await _postService.GetAllPosts());
        }

        [HttpGet("posts/{postId}")]
        public async Task<ActionResult<ServiceResponse<GetPostResponseDto>>> GetSingle(int postId)
        {
            return Ok(await _postService.GetPostById(postId));
        }
        [Authorize]
        [HttpPost("post")]
        public async Task<ActionResult<ServiceResponse<List<GetPostResponseDto>>>> AddPost(AddPostRequestDto newPost)
        {
            return Ok(await _postService.AddPost(newPost));
        }
        [Authorize]
        [HttpPut("post/{postId}")]
        public async Task<ActionResult<ServiceResponse<List<GetPostResponseDto>>>> UpdatePost(int postId, UpdatePostRequestDto updatedPost)
        {
            var response = await _postService.UpdatePost(postId, updatedPost);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [Authorize]
        [HttpDelete("post/{postId}")]
        public async Task<ActionResult<ServiceResponse<GetPostResponseDto>>> DeletePost(int postId)
        {
            var response = await _postService.DeletePost(postId);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}