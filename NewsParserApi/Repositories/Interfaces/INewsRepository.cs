using NewsParserApi.Entities;
using NewsParserApi.Models.NewsDto;

namespace NewsParserApi.Repositories.Interfaces
{
    public interface INewsRepository : IBaseRepository<News>
    {
        IEnumerable<News> AddNewsWithUniqueTitles(IEnumerable<News> news);
        IEnumerable<NewsPreviewList> GetWithPagination(int count, int start, string? currentUsername = null);
        NewsPreviewList? GetByIdWithLikes(int id, string? currentUsername = null);
        IEnumerable<string> GetAllTitles();
        void LikeNews(int newsId, string username, bool? isLike);
        News? GetByIdWithIncludes(int id);
    }
}
