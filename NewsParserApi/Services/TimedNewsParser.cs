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
        private bool bigRequest = false;

        public TimedNewsParser(ILogger<TimedNewsParser> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _newsRepository = factory.CreateScope().ServiceProvider.GetRequiredService<INewsRepository>();

            bigRequest = _newsRepository.IsEmpty();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Parser Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(24)); // daily
            return Task.CompletedTask;
        }

        private List<News> ParseInvestingWebApp(int postsCount, int page = 1, IEnumerable<string>? existingTitles = null)
        {
            List<News> news = new List<News>();
            int i = 0;
            while (i <= postsCount)
            {
                string url = $"https://www.investing.com/news/economy/{page}";

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(ParsesHelper.CallUrl(url).Result);

                var newsList = htmlDoc.DocumentNode.SelectNodes("//section[@id=\"leftColumn\"]/div[@class=\"largeTitle\"]/article[@class=\"js-article-item articleItem   \"]");

                foreach (var element in newsList)
                {
                    i++;
                    if (i > postsCount)
                        break;
                    //Get the title from the news list
                    var title = element.SelectSingleNode("div/a").InnerText.Trim();
                    if (existingTitles != null) 
                        if (existingTitles.Contains(title)) //Check if the news already exists
                            continue;

                    var singleNewsUrl = "https://www.investing.com" + element.SelectSingleNode("div/a").Attributes["href"].Value.Trim();
                    var htmlOneNews = new HtmlDocument();
                    htmlOneNews.LoadHtml(ParsesHelper.CallUrl(singleNewsUrl).Result);

                    var newsSection = htmlOneNews.DocumentNode.SelectSingleNode("//section[@id=\"leftColumn\"]");

                    //Get the dateTime from the single news page 
                    var dateStr = newsSection.SelectSingleNode("div[@class=\"contentSectionDetails\"]/span").InnerText.Trim().Replace(" ET", "");
                    int startIndexOfDate = dateStr.IndexOf('(');
                    if (startIndexOfDate != -1)
                        dateStr = dateStr.Substring(startIndexOfDate + 1).Replace(")", "");
                    var dt = DateTime.Parse(dateStr);


                    //Get the conent from the single news page 
                    List<string> paragraphs = new List<string>();
                    var pElements = newsSection.SelectNodes("div[@class=\"WYSIWYG articlePage\"]/p");
                    foreach (var p in pElements)
                        paragraphs.Add(p.InnerText);

                    var json = JsonSerializer.Serialize(paragraphs);

                    news.Add(new News()
                    {
                        Title = title,
                        ImageUrl = newsSection.SelectSingleNode("div[@class=\"WYSIWYG articlePage\"]/div[@id=\"imgCarousel\"]/img")?.Attributes["src"].Value,
                        Url = singleNewsUrl,
                        Text = element.SelectNodes("div/p")[0].InnerText,
                        Date = dt,
                        Content = json
                    });

                    Console.WriteLine($"Added new {news.Count}");
                }
                page++;
            }

            return news;
        }

        private async void ParseAndSaveUniqueNewsAsync(int requestCount)
        {
            var allTitles = _newsRepository.GetAllTitles();

            var news = ParseInvestingWebApp(requestCount, existingTitles: allTitles);

            var addedNews = _newsRepository.AddNewsWithUniqueTitles(news);
            await _newsRepository.SaveChangesAsync();


            _logger.LogInformation("--- Timed Parser Service made a request to https://www.investing.com/\n" +
                                    $"--- Number of new news: {addedNews.Count()}");

            Console.WriteLine($"\n\n ==== Number of new news: {addedNews.Count()} ===== \n\n");
        }

        private void DoWork(object? state)
        {
            if(!bigRequest)
                ParseAndSaveUniqueNewsAsync(20);
            else
                ParseAndSaveUniqueNewsAsync(100);
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
