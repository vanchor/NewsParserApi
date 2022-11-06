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
        public ActionResult<NewsById> GetNewsById(int id)
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

            var newsVM = new NewsById()
            {
                Id = newsInDb.Id,
                Title = newsInDb.Title,
                Date = newsInDb.Date,
                Text = newsInDb.Text,
                ImageUrl = newsInDb.ImageUrl,
                Url = newsInDb.Url,
                LikesCount = newsInDb.LikeDislike.Count(ld => ld.isLike == true),
                DislikesCount = newsInDb.LikeDislike.Count(ld => ld.isLike == false),
                likedByCurrentUser = newsInDb.LikeDislike?.FirstOrDefault(x => x.Username == currentUsername)?.isLike,
                Content = JsonSerializer.Deserialize<List<string>>(newsInDb.Content)
            };

            foreach (var comment in newsInDb.Comments)
                newsVM.Comments.Add(new CommentVM(comment, currentUsername));

            return Ok(newsVM);
        }

        [HttpPost("{id}/likeDislike"), Authorize]
        public ActionResult<NewsPreviewList> LikeNews(int id, bool? isLike)
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
                return BadRequest(ex.Message);
            }

            var news = _newsRepository.GetByIdWithLikes(id, currentUsername);

            return Ok(news);
        }

        [HttpPost("{id}/addComment"), Authorize]
        public ActionResult addComment(int id, string commentText)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

            try
            {
                var newsInDb = _newsRepository.GetById(id);

                if (newsInDb == null)
                    return NotFound("No news with this id");

                Comment comment = new Comment()
                {
                    NewsId = id,
                    Date = DateTime.Now,
                    Text = commentText,
                    Username = currentUserName
                };

                _commentRepository.Add(comment);
                _commentRepository.SaveChanges();
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
