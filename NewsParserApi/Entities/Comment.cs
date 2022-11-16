using System.ComponentModel.DataAnnotations.Schema;

namespace NewsParserApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("User")]
        public string Username { get; set; }
        public User User { get; set; }

        public int? NewsId { get; set; }
        public News? CommentedNews { get; set; }

        public int? CommentId { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikeDislike>? LikeDislike { get; set; }
    }
}
