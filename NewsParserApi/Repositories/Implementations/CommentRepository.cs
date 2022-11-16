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

        public Comment? getByIdWithIncludes(int id)
        {
            return _context.Comments.Select(c => new Comment
                {
                    Id = c.Id,
                    Text = c.Text,
                    Date = c.Date,
                    Username = c.Username,
                    LikeDislike = c.LikeDislike,
                    Comments = c.Comments.Select(cl2 => new Comment
                    {
                        Id = cl2.Id,
                        Text = cl2.Text,
                        Date = cl2.Date,
                        Username = cl2.Username,
                        LikeDislike = cl2.LikeDislike
                    }).OrderBy(x => x.Date).ToList()
                }).FirstOrDefault(x => x.Id == id);
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
