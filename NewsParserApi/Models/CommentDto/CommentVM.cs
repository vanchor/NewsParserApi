using NewsParserApi.Entities;

namespace NewsParserApi.Models.CommentDto
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        public List<CommentVM>? Comments { get; set; } = new List<CommentVM>();

        public int LikesCount { get; set; } = 0;
        public int DislikesCount { get; set; } = 0;
        public bool? LikedByCurrentUser { get; set; } = null;

        public CommentVM()
        {
        }

        public CommentVM(Comment comment, string? currentUsername = null)
        {
            Id = comment.Id;
            Text = comment.Text;
            Date = comment.Date;
            Username = comment.Username;

            if (comment.LikeDislike != null) {
                LikesCount = comment.LikeDislike.Count(x => x.isLike == true);
                DislikesCount = comment.LikeDislike.Count(x => x.isLike == false);
                LikedByCurrentUser = comment.LikeDislike.FirstOrDefault(x => x.Username == currentUsername)?.isLike;
            }

            if (comment.Comments != null)
                foreach (var c in comment.Comments)
                    Comments.Add(new CommentVM(c, currentUsername));
        }
    }
}
