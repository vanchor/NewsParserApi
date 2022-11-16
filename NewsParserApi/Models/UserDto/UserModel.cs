using System.ComponentModel.DataAnnotations;

namespace NewsParserApi.Models.UserDto
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
