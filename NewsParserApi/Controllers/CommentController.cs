using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Repositories.Interfaces;
using System.Security.Claims;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost("{id}/likeDislike"), Authorize]
        public ActionResult LikeNews(int id, bool isLike)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

            try
            {
                var newsInDb = _commentRepository.GetById(id);

                if (newsInDb == null)
                    return NotFound("No comment with this id");

                _commentRepository.LikeComment(id, currentUserName, isLike);
                _commentRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost("{id}/addComment"), Authorize]
        public ActionResult AddComment(int id, string commentText)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

            try
            {
                var commentInDb = _commentRepository.GetById(id);

                if (commentInDb == null)
                    return NotFound("No comment with this id");

                Comment comment = new Comment()
                {
                    Date = DateTime.Now,
                    Text = commentText,
                    Username = currentUserName,
                    CommentId = commentInDb.Id
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
    }
}
