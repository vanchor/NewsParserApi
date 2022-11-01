using NewsParserApi.Entities;

namespace NewsParserApi.Models.CommentDto
{
    public class CommentWithoutComents
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        public CommentWithoutComents()
        {

        }

        public CommentWithoutComents(Comment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            Date = comment.Date;
            Username = comment.Username;
        }
    }
}
