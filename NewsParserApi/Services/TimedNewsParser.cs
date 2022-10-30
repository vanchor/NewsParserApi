using HtmlAgilityPack;
using NewsParserApi.Entities;
using NewsParserApi.Helpers;
using NewsParserApi.Models;
using NewsParserApi.Repositories.Interfaces;
using System.Globalization;
using System.Text.Json;
using System.Web;

namespace NewsParserApi.Services
{
    public class TimedNewsParser : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedNewsParser> _logger;
        private Timer? _timer = null;
        private INewsRepository _newsRepository;

        public TimedNewsParser(ILogger<TimedNewsParser> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _newsRepository = factory.CreateScope().ServiceProvider.GetRequiredService<INewsRepository>();

            if (_newsRepository.IsEmpty())
                ParseAndSaveUniqueNews(300);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Parser Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(24)); // daily
            return Task.CompletedTask;
        }

        private string DeserializeAndDecodeHtml(string json)
        {
            var investorsWebApi = JsonSerializer.Deserialize<InvestorsWebApi>(json);

            if (investorsWebApi == null)
                throw new ArgumentNullException();

            return HttpUtility.HtmlDecode(investorsWebApi.html) ?? "";
        }

        private List<News> ParseInvestorsHtml(string html, IEnumerable<string>? existingTitles = null) // Parser for https://www.investors.com/
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var newsList = htmlDoc.DocumentNode.SelectNodes("//li");
            List<News> news = new List<News>();

            foreach (var element in newsList)
            {
                var title = element.SelectSingleNode("h3/a").InnerText.Trim();
                if (existingTitles.Contains(title))
                    continue;

                var url = element.SelectSingleNode("h3/a").Attributes["href"].Value.Trim();
                //var response = ParsesHelper.CallUrl(url).Result;
                //var htmlOneNews = new HtmlDocument();
                //htmlOneNews.LoadHtml(response);
                //var newsArticle = htmlOneNews.DocumentNode.SelectNodes("//article")[0];

                //var dt = DateTime.ParseExact(
                //    s: newsArticle.SelectNodes("header//div/ul/li")[1].InnerText.Trim().Replace("ET ", ""),
                //    format: "hh:mm tt MM/dd/yyyy", 
                 //   provider: CultureInfo.InvariantCulture);

               // List<string> paragraphs = new List<string>();
               // var pElements = newsArticle.SelectNodes("div/p");
               // foreach(var p in pElements)
                //    paragraphs.Add(p.InnerText);

              //  var json = JsonSerializer.Serialize(paragraphs);

                news.Add(new News()
                {
                    Title = title,
                    ImageUrl = element.SelectSingleNode("img")?.Attributes["src"].Value.Replace("-150x150", "").Trim(),
                    Url = url,
                    Text = element.SelectNodes("p")[1].InnerText.Trim(),
                   // Date = dt,
                  //  Content = json
                });
            }
            return news;
        }

        private List<News> ParseInvestorsWebApp(int posts_per_page = 5, int page = 0, IEnumerable<string>? existingTitles = null)
        {
            string url = $"https://www.investors.com/wp-admin/admin-ajax.php?slug=economy&canonical_url=https%3A%2F%2Fwww.investors.com%2Fcategory%2Fnews%2Feconomy%2F&posts_per_page={posts_per_page}&page={page}&category=economy&order=DESC&orderby=date&action=alm_get_posts";

            var response = ParsesHelper.CallUrl(url).Result;
            var news = ParseInvestorsHtml(DeserializeAndDecodeHtml(response), existingTitles);
            return news;
        }

        private async void ParseAndSaveUniqueNews(int requestCount)
        {
            var news = ParseInvestorsWebApp(requestCount, existingTitles: _newsRepository.GetAllTitles());

            var addedNews = _newsRepository.AddNewsWithUniqueTitles(news);
            await _newsRepository.SaveChangesAsync();


            _logger.LogInformation("--- Timed Parser Service made a request to https://www.investors.com/\n" +
                                    $"--- Number of new news: {addedNews.Count()}");

            Console.WriteLine($"\n\n ==== Number of new news: {addedNews.Count()} ===== \n\n");
        }

        private void DoWork(object? state)
        {
            ParseAndSaveUniqueNews(10);
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
