using NewsParserApi.Entities;
using NewsParserApi.Models.NewsDto;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface INewsRepository : IBaseRepository<News>
    {
        IEnumerable<News> AddNewsWithUniqueTitles(IEnumerable<News> news);
        IEnumerable<NewsPreviewList> GetWithPagination(int count, int start, string? currentUsername = null);
        IEnumerable<String> GetAllTitles();
        void LikeNews(int newsId, string username, bool isLike);
    }
}
