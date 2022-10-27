using NewsParserApi.Data;
using NewsParserApi.Entities;
using NewsParserApi.Repositories.Interfaces;

namespace NewsParserApi.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(NewsApiDbContext context) : base(context)
        {
        }

        public User? GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
