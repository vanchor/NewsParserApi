using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Models.UserDto;
using NewsParserApi.Services;
using System.Security.Claims;
using System.Net;

namespace NewsParserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponse>> Register([FromBody] UserModel request)
        {
            var response = await _userService.Register(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.Conflict:
                    return Conflict(response.Description);
                case HttpStatusCode.UnprocessableEntity:
                    return UnprocessableEntity(response.Description);
                default:
                    return BadRequest(response.Description);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticateRequest request)
        {
            var response = _userService.Authenticate(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Data;
                case HttpStatusCode.UnprocessableEntity:
                    return UnprocessableEntity(response.Description);
                default:
                    return BadRequest(response.Description);
            }
        }

        [HttpGet("Test"), Authorize]
        public ActionResult Test()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.Name).Value;
            return Ok(currentUserName);
        }
    }
}
