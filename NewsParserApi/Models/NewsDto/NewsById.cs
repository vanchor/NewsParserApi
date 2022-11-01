using NewsParserApi.Entities;
using NewsParserApi.Models.CommentDto;

namespace NewsParserApi.Models.NewsDto
{
    public class NewsById : NewsPreviewList
    {
        public List<string>? Content { get; set; }
        public List<CommentVM>? Comments { get; set; } = new List<CommentVM>();
    }
}
