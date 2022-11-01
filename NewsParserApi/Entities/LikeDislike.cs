using System.ComponentModel.DataAnnotations.Schema;

namespace NewsParserApi.Entities
{
    public class LikeDislike
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public int? NewsId { get; set; }
        public int? CommentId { get; set; }
        public bool isLike { get; set; }

        public News? News { get; set; }
        public Comment? Comment { get; set; }
        public User User { get; set; }
    }
}
