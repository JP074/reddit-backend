using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, GetPostResponseDto>();
            CreateMap<AddPostRequestDto, Post>();
            CreateMap<Comment, GetCommentResponseDto>();
            CreateMap<AddCommentRequestDto, Comment>();
        }
    }
}