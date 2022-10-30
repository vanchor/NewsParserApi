using NewsParserApi.Data;
using NewsParserApi.Entities;
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
