using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Models.CommentDto;
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
        public ActionResult<CommentVM> LikeComment(int id, bool isLike)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

            var newsInDb = _commentRepository.GetById(id);

            if (newsInDb == null)
                return NotFound("No comment with this id");

            try
            {
                _commentRepository.LikeComment(id, currentUserName, isLike);
                _commentRepository.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }

            var response = new CommentVM(_commentRepository.getByIdWithIncludes(id), currentUserName);

            return Ok(response);
        }

        [HttpPost("{id}/addComment"), Authorize]
        public ActionResult<CommentVM> AddComment(int id, string commentText)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;

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

            var response = new CommentVM(_commentRepository.getByIdWithIncludes(id), currentUserName);

            return Ok(response);
        }
    }
}
