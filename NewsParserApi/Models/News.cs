namespace NewsParserApi.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string Url { get; set; }
    }
}
