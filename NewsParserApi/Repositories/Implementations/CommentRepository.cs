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

        public void LikeComment(int commentId, string username, bool isLike)
        {
            var inDb = _context.LikeDislike.FirstOrDefault(x => (x.CommentId == commentId)
                                            && (x.Username == username));
            if (inDb != null)
            {
                if (inDb.isLike == isLike)
                    throw new ArgumentException("There is already a record with this data");
                else
                {
                    inDb.isLike = isLike;
                    _context.Entry(inDb).Property(i => i.isLike).IsModified = true;
                }
            }
            else
            {
                _context.LikeDislike.Add(new LikeDislike()
                {
                    Username = username,
                    CommentId = commentId,
                    isLike = isLike
                });
            }
        }
    }
}
