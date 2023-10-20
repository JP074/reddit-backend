using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Services
{
    public interface IPostService
    {
        Task<ServiceResponse<List<GetPostResponseDto>>> GetAllPosts();
        Task<ServiceResponse<GetPostResponseDto>> GetPostById(int id);
        Task<ServiceResponse<List<GetPostResponseDto>>> AddPost(AddPostRequestDto newPost);
        Task<ServiceResponse<GetPostResponseDto>> UpdatePost(int postId, UpdatePostRequestDto updatedPost);
        Task<ServiceResponse<List<GetPostResponseDto>>> DeletePost(int id);
    }
}