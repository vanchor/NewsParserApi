namespace NewsParserApi.Models.CommentDto
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        //public IEnumerable<CommentVM>? Comments { get; set; }
    }
}
