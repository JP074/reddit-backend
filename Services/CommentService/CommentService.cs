using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetCommentResponseDto>>> AddComment(AddCommentRequestDto newComment, int postId)
        {
            var serviceResponse = new ServiceResponse<List<GetCommentResponseDto>>();
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
                if (post is null)
                {
                    throw new Exception("Post not found.");
                }

                var comment = _mapper.Map<Comment>(newComment);
                comment.PostId = postId;
                comment.Post = post;
                comment.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
                comment.CreatedAt = DateTime.Now;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Comments.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCommentResponseDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCommentResponseDto>>> DeleteComment(int commentId)
        {
            var serviceResponse = new ServiceResponse<List<GetCommentResponseDto>>();

            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId && c.User!.Id == GetUserId());

                if (comment is null)
                {
                    throw new Exception("The comment is not found.");
                }

                _context.Comments.Remove(comment);

                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Comments.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCommentResponseDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCommentResponseDto>>> GetAllComments(int postId)
        {
            var serviceResponse = new ServiceResponse<List<GetCommentResponseDto>>();
            var dbComments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            serviceResponse.Data = dbComments.Select(c => _mapper.Map<GetCommentResponseDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCommentResponseDto>> UpdateComment(int commentId, UpdateCommentRequestDto updatedComment)
        {
            var serviceResponse = new ServiceResponse<GetCommentResponseDto>();

            try
            {
                var comment = await _context.Comments.Include(c => c.User).FirstAsync(c => c.Id == commentId);

                if (comment is null || comment.User!.Id != GetUserId())
                {
                    throw new Exception("The comment is not found.");
                }

                comment.Content = updatedComment.Content;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCommentResponseDto>(comment);
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