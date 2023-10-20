using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_backend.Dtos.Comment
{
    public class AddCommentRequestDto
    {
        public string Content { get; set; } = string.Empty;

    }
}