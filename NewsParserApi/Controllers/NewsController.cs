using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Data;
using NewsParserApi.Models;
using System.Text.Json;
using System.Web;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsApiDbContext _context;

        public NewsController(NewsApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNews(int posts_per_page, int page = 0)
        {
            //if (_context.News.Count() == 0) 
            //{
            //    var news = ParseInvestorsWebApp(posts_per_page);
            //    await _context.News.AddRangeAsync(news);
            //    await _context.SaveChangesAsync();
            //    return news;
            //}

            return _context.News.Take(posts_per_page).ToList();
        }

        [HttpPost]
        public ActionResult<News> AddNews(News n)
        {
            _context.News.Add(n);
            _context.SaveChanges();
            return n;
        }
    }
}
