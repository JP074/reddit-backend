using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        public async Task<ServiceResponse<List<GetPostResponseDto>>> AddPost(AddPostRequestDto newPost)
        {
            var serviceResponse = new ServiceResponse<List<GetPostResponseDto>>();
            var post = _mapper.Map<Post>(newPost);
            post.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            post.UserId = GetUserId();
            post.CreatedAt = DateTime.Now;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Posts.Where(p => p.User!.Id == GetUserId()).Select(p => _mapper.Map<GetPostResponseDto>(p)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostResponseDto>>> DeletePost(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetPostResponseDto>>();

            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id && p.User!.Id == GetUserId());

                if (post is null)
                {
                    throw new Exception("Post not found.");
                }

                var commentsToDelete = _context.Comments.Where(c => c.PostId == id);

                _context.Comments.RemoveRange(commentsToDelete);
                _context.Posts.Remove(post);

                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Posts.Where(p => p.User!.Id == GetUserId()).Select(p => _mapper.Map<GetPostResponseDto>(p)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostResponseDto>>> GetAllPosts()
        {
            var serviceResponse = new ServiceResponse<List<GetPostResponseDto>>();
            var dbPosts = await _context.Posts.ToListAsync();
            serviceResponse.Data = dbPosts.Select(p => _mapper.Map<GetPostResponseDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostResponseDto>> GetPostById(int id)
        {
            var serviceResponse = new ServiceResponse<GetPostResponseDto>();
            var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            serviceResponse.Data = _mapper.Map<GetPostResponseDto>(dbPost);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostResponseDto>> UpdatePost(int id, UpdatePostRequestDto updatedPost)
        {
            var serviceResponse = new ServiceResponse<GetPostResponseDto>();

            try
            {
                var post = await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);

                if (post is null || post.User!.Id != GetUserId())
                {
                    throw new Exception("The post is not found.");
                }

                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPostResponseDto>(post);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}