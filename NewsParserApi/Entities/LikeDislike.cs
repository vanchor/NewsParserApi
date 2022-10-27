using System.ComponentModel.DataAnnotations.Schema;

namespace NewsParserApi.Entities
{
    public class LikeDislike
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public int NewsId { get; set; }
        public bool isLike { get; set; }

        public News News { get; set; }
        public User User { get; set; }
    }
}
