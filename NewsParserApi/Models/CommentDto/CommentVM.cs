using NewsParserApi.Entities;

namespace NewsParserApi.Models.CommentDto
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        public List<CommentWithoutComents>? Comments { get; set; } = new List<CommentWithoutComents>();

        public CommentVM()
        {
        }

        public CommentVM(Comment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            Date = comment.Date;
            Username = comment.Username;

            if(comment.Comments != null)
                foreach (var c in comment.Comments)
                    Comments.Add(new CommentWithoutComents(c));
        }
    }
}
