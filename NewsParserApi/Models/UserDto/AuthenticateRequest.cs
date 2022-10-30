using System.ComponentModel.DataAnnotations;

namespace NewsParserApi.Models.UserDto
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
