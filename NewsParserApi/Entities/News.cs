using System.Text.Json.Serialization;

namespace NewsParserApi.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public string Url { get; set; }
        public string? Content { get; set; }

        [JsonIgnore]
        public DateTime ParsedDate { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<LikeDislike>? LikeDislike { get; set; }
    }
}
