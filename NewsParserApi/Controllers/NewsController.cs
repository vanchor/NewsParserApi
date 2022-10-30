using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Models.NewsDto;
using NewsParserApi.Repositories.Interfaces;
using System.Security.Claims;

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
        public ActionResult<IEnumerable<NewsPreviewList>> GetNews(int count, int start = 0)
        {
            string? currentUsername = null;
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = User;
                currentUsername = currentUser.FindFirst(ClaimTypes.Name).Value;
            }

            return _newsRepository.GetWithPagination(count, start, currentUsername).ToList();
        }

        [HttpPost("{id}/likeDislike"), Authorize]
        public ActionResult LikeNews(int id, bool isLike)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

            try
            {
                _newsRepository.LikeNews(id, currentUserName, isLike);
                _newsRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost, Authorize]
        public ActionResult<News> AddNews(News n)
        {
            _newsRepository.Add(n);
            _newsRepository.SaveChanges();
            return n;
        }
    }
}
