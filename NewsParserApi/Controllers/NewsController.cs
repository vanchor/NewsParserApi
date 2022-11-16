using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Models.NewsDto;
using NewsParserApi.Repositories.Interfaces;
using System.Text.Json;
using System.Security.Claims;
using NewsParserApi.Models.CommentDto;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;
        private readonly ICommentRepository _commentRepository;

        public NewsController(INewsRepository newsRepository, ICommentRepository commentRepository)
        {
            _newsRepository = newsRepository;
            _commentRepository = commentRepository;
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

            return Ok(_newsRepository.GetWithPagination(count, start, currentUsername));
        }

        [HttpGet("{id}")]
        public ActionResult<NewsVM> GetNewsById(int id)
        {
            string? currentUsername = null;
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = User;
                currentUsername = currentUser.FindFirst(ClaimTypes.Name).Value;
            }
            var newsInDb = _newsRepository.GetByIdWithIncludes(id);

            if (newsInDb == null)
                return NotFound();

            var newsVM = new NewsVM(newsInDb, currentUsername);

            return Ok(newsVM);
        }

        [HttpPost("{id}/likeDislike"), Authorize]
        public ActionResult<NewsPreviewList> LikeNews(int id, bool? isLike = null)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUsername = currentUser.FindFirst(ClaimTypes.Name).Value;

            try
            {
                var newsInDb = _newsRepository.GetById(id);

                if (newsInDb == null)
                    return NotFound("No news with this id");

                _newsRepository.LikeNews(id, currentUsername, isLike);
                _newsRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }

            var news = _newsRepository.GetByIdWithLikes(id, currentUsername);

            return Ok(news);
        }

        [HttpPost("{id}/addComment"), Authorize]
        public ActionResult<NewsVM> addComment(int id, string commentText)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUsername = currentUser.FindFirst(ClaimTypes.Name).Value;

            var newsInDb = _newsRepository.GetById(id);

            if (newsInDb == null)
                return NotFound("No news with this id");

            Comment comment = new Comment()
            {
                NewsId = id,
                Date = DateTime.Now,
                Text = commentText,
                Username = currentUsername
            };

            _commentRepository.Add(comment);
            _commentRepository.SaveChanges();


            var newsVM = new NewsVM(_newsRepository.GetByIdWithIncludes(id), currentUsername);

            return Ok(newsVM);
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
