using NewsParserApi.Data;
using NewsParserApi.Models;
using NewsParserApi.Repositories.Interfaces;

namespace NewsParserApi.Repositories.Implementations
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(NewsApiDbContext context) : base(context)
        {
        }
    }
}
