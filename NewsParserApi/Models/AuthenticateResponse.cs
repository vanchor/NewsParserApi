using NewsParserApi.Entities;

namespace NewsParserApi.Models
{
    public class AuthenticateResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Username = user.Username;
            Email = user.Email;
            Token = token;
        }
    }
}
