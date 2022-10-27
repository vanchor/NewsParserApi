using NewsParserApi.Entities;
using NewsParserApi.Models;

namespace NewsParserApi.Services
{
    public interface IUserService
    {
        Task<BaseResponse<AuthenticateResponse>> Register(UserModel userModel);
        BaseResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        User GetById(string id);
    }
}
