using Microsoft.EntityFrameworkCore;
using NewsParserApi.Data;
using NewsParserApi.Entities;
using NewsParserApi.Models.NewsDto;
using NewsParserApi.Repositories.Interfaces;

namespace NewsParserApi.Repositories.Implementations
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(NewsApiDbContext context) : base(context)
        {
        }

        public News? GetByIdWithIncludes(int id)
        {
            return _context.News
                .Include(n => n.LikeDislike)
                .Include(n => n.Comments)
                .ThenInclude(x => x.Comments)
                .FirstOrDefault(n => n.Id == id);
        }

        public IEnumerable<News> AddNewsWithUniqueTitles(IEnumerable<News> news)
        {
            var titlesInDb = GetAllTitles();
            var newsWithUniqueTitles = news.DistinctBy(x => x.Title);

            newsWithUniqueTitles = newsWithUniqueTitles.Where(x => !titlesInDb.Contains(x.Title));
            base.AddRange(newsWithUniqueTitles);

            return newsWithUniqueTitles;
        }

        public IEnumerable<string> GetAllTitles()
        {
            return _context.News.Select(x => x.Title).ToList();
        }

        public IEnumerable<NewsPreviewList> GetWithPagination(int count, int start, string? currentUsername = null)
        {
            return _context.News
                .Select(x => new NewsPreviewList()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    Date = x.Date,
                    ImageUrl = x.ImageUrl,
                    Url = x.Url,
                    DislikesCount = x.LikeDislike.Count(x => x.isLike == false),
                    LikesCount = x.LikeDislike.Count(x => x.isLike == true),
                    likedByCurrentUser = x.LikeDislike.FirstOrDefault(x => x.Username == currentUsername).isLike
                })
                .OrderByDescending(n => n.Date)
                .Skip(start)
                .Take(count)
                .AsNoTracking()
                .ToList();
        }

        public void LikeNews(int newsId, string username, bool isLike)
        {
            var inDb = _context.LikeDislike.FirstOrDefault(x => (x.NewsId == newsId)
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
                    NewsId = newsId,
                    isLike = isLike
                });
            }
        }
    }
}
