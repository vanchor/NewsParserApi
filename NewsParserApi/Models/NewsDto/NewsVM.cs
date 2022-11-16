using NewsParserApi.Entities;
using NewsParserApi.Models.CommentDto;
using System.Text.Json;

namespace NewsParserApi.Models.NewsDto
{
    public class NewsVM : NewsPreviewList
    {
        public List<string>? Content { get; set; }
        public List<CommentVM>? Comments { get; set; } = new List<CommentVM>();

        public NewsVM() { }
        public NewsVM(News news, string? currentUsername = null)
        {
            Id = news.Id;
            Title = news.Title;
            Date = news.Date;
            Text = news.Text;
            ImageUrl = news.ImageUrl;
            Url = news.Url;
            LikesCount = news.LikeDislike.Count(ld => ld.isLike == true);
            DislikesCount = news.LikeDislike.Count(ld => ld.isLike == false);
            likedByCurrentUser = news.LikeDislike?.FirstOrDefault(x => x.Username == currentUsername)?.isLike;
            Content = JsonSerializer.Deserialize<List<string>>(news.Content);

            foreach (var comment in news.Comments)
                Comments.Add(new CommentVM(comment, currentUsername));
        }

    }
}
