using NewsParserApi.Entities;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetById(string id);
        Task<User?> GetByIdAsync(string id);
    }
}
