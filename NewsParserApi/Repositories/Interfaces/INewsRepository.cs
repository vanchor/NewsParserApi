using NewsParserApi.Models;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface INewsRepository : IBaseRepository<News>
    {
        IEnumerable<News> GetWithPagination(int count, int page);
    }
}
