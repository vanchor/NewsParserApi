using NewsParserApi.Entities;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        void LikeComment(int commentId, string username, bool isLike);
        Comment? getByIdWithIncludes(int id);
    }
}
