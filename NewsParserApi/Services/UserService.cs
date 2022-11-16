using NewsParserApi.Helpers;
using NewsParserApi.Entities;
using NewsParserApi.Repositories.Interfaces;
using NewsParserApi.Models.UserDto;
using NewsParserApi.Models;

namespace NewsParserApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public BaseResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var userInDb = _userRepository.GetById(model.Username);

            if (userInDb == null || 
                !HashPasswordHelper.VerifyPasswordHash(model.Password, userInDb.PasswordHash, userInDb.PasswordSalt))
            {
                return new BaseResponse<AuthenticateResponse>(){
                    StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                    Description = "Username or password is incorrect."
                };
            }

            var token = _configuration.GenerateJwtToken(userInDb);

            return new BaseResponse<AuthenticateResponse>(){
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = new AuthenticateResponse(userInDb, token)
            };
        }

        public User GetById(string id)
        {
            return _userRepository.GetById(id);
        }

        public async Task<BaseResponse<AuthenticateResponse>> Register(UserModel userModel)
        {
            
            if(_userRepository.GetById(userModel.Username) != null)
                return new BaseResponse<AuthenticateResponse>(){
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Description = "User already exists."
                };

            HashPasswordHelper.CreatePasswordHash(userModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Username = userModel.Username,
                Email = userModel.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var response = Authenticate(new AuthenticateRequest
            {
                Username = userModel.Username,
                Password = userModel.Password
            });

            return response;
        }
    }
}
