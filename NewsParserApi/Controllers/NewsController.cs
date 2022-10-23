using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Models;
using NewsParserApi.Repositories.Interfaces;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<News>> GetNews(int posts_per_page, int page = 0)
        {
            return _newsRepository.GetWithPagination(posts_per_page, page).ToList();
        }

        [HttpPost]
        public ActionResult<News> AddNews(News n)
        {
            _newsRepository.Add(n);
            _newsRepository.SaveChanges();
            return n;
        }
    }
}
