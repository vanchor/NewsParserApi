using HtmlAgilityPack;
using NewsParserApi.Models;
using System.Text.Json;
using System.Web;

namespace NewsParserApi.Services
{
    public class TimedNewsParser : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedNewsParser> _logger;
        private Timer? _timer = null;

        public TimedNewsParser(ILogger<TimedNewsParser> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Parser Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(24)); // daily
            return Task.CompletedTask;
        }


        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)");
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        private string DeserializeAndDecodeHtml(string json)
        {
            var investorsWebApi = JsonSerializer.Deserialize<InvestorsWebApi>(json);

            if (investorsWebApi == null)
                throw new ArgumentNullException();

            return HttpUtility.HtmlDecode(investorsWebApi.html) ?? "";
        }

        private List<News> ParseInvestorsHtml(string html) // Parser for https://www.investors.com/
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var newsList = htmlDoc.DocumentNode.SelectNodes("//li");
            List<News> news = new List<News>();

            foreach (var element in newsList)
            {
                news.Add(new News()
                {
                    ImageUrl = element.SelectSingleNode("img")?.Attributes["src"].Value.Replace("-150x150", "").Trim(),
                    Title = element.SelectSingleNode("h3/a").InnerText.Trim(),
                    Url = element.SelectSingleNode("h3/a").Attributes["href"].Value.Trim(),
                    Text = element.SelectNodes("p")[1].InnerText.Trim(),
                    Date = element.SelectNodes("p")[0].InnerText.Trim()
                });
            }

            return news;
        }

        private List<News> ParseInvestorsWebApp(int posts_per_page = 5, int page = 0)
        {
            string url = $"https://www.investors.com/wp-admin/admin-ajax.php?slug=economy&canonical_url=https%3A%2F%2Fwww.investors.com%2Fcategory%2Fnews%2Feconomy%2F&posts_per_page={posts_per_page}&page={page}&category=economy&order=DESC&orderby=date&action=alm_get_posts";

            var response = CallUrl(url).Result;
            var news = ParseInvestorsHtml(DeserializeAndDecodeHtml(response));
            return news;
        }

        private void DoWork(object? state)
        {
            var news = ParseInvestorsWebApp(1);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", news);

            Console.WriteLine("\n\n ==== AGA ===== \n\n");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Parser Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
