namespace NewsParserApi.Models
{
    public class Meta
    {
        public int postcount { get; set; }
        public int totalposts { get; set; }
        public bool debug { get; set; }
    }

    public class InvestorsWebApi
    {
        public string? html { get; set; }
        public Meta? meta { get; set; }
    }
}
