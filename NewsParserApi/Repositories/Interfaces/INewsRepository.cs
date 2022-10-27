using NewsParserApi.Entities;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface INewsRepository : IBaseRepository<News>
    {
        IEnumerable<News> AddNewsWithUniqueTitles(IEnumerable<News> news);
        IEnumerable<News> GetWithPagination(int count, int page);
        IEnumerable<String> GetAllTitles();
    }
}
