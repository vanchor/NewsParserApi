using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Models;
using System.Text.Json;
using System.Web;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        class Meta
        {
            public int postcount { get; set; }
            public int totalposts { get; set; }
            public bool debug { get; set; }
        }

        class investApi {
            public string html { get; set; }
            public Meta meta { get; set; }
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)");
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        private List<News> ParseHtml(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var elementsList = htmlDoc.DocumentNode.SelectNodes("//li");

            List<News> news = new List<News>();

            foreach (var element in elementsList)
            {
                news.Add(new News()
                {
                    ImageUrl = element.SelectSingleNode("img").Attributes["src"].Value.Replace("-150x150", "").Trim(),
                    Title = element.SelectSingleNode("h3/a").InnerText.Trim(),
                    Url = element.SelectSingleNode("h3/a").Attributes["href"].Value.Trim(),
                    Text = element.SelectNodes("p")[1].InnerText.Trim(),
                    Date = element.SelectNodes("p")[0].InnerText.Trim()
                });
            }

            return news;
        }

        [HttpGet]
        public ActionResult<List<News>> GetNews()
        {
            string url = "https://www.investors.com/wp-admin/admin-ajax.php?slug=economy&canonical_url=https%3A%2F%2Fwww.investors.com%2Fcategory%2Fnews%2Feconomy%2F&posts_per_page=20&page=0&category=economy&order=DESC&orderby=date&action=alm_get_posts";
            //string url = "https://www.investors.com/category/news/economy";

            var response = CallUrl(url).Result;

            var encoded = HttpUtility.HtmlDecode(JsonSerializer.Deserialize<investApi>(response).html);

            var news = ParseHtml(encoded);
            return news;
        }
    }
}
