using NewsParserApi.Entities;

namespace NewsParserApi.Models.NewsDto
{
    public class NewsById : NewsPreviewList
    {

        public List<string>? Content { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
