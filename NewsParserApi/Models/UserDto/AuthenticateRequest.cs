using System.ComponentModel.DataAnnotations;

namespace NewsParserApi.Models.UserDto
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
