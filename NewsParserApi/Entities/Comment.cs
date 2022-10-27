namespace NewsParserApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int NewsId { get; set; }
        public News CommentedNews { get; set; }
    }
}
