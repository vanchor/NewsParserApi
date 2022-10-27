using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsParserApi.Entities;
using NewsParserApi.Models;
using NewsParserApi.Services;

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
        public async Task<ActionResult<AuthenticateResponse>> Register(UserModel request)
        {
            var response = await _userService.Register(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(new { message = response.Description });

            return response.Data;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login(AuthenticateRequest request)
        {
            var response = _userService.Authenticate(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(new { message = response.Description });

            return response.Data;
        }

        [Authorize]
        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("Dicki");
        }
    }
}
