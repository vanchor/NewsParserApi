using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NewsParserApi.Data;
using NewsParserApi.Models;
using NewsParserApi.Repositories.Interfaces;

namespace NewsParserApi.Repositories.Implementations
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(NewsApiDbContext context) : base(context)
        {
        }

        public IEnumerable<News> AddNewsWithUniqueTitles(IEnumerable<News> news)
        {
            var titlesInDb = GetAllTitles();
            var newsWithUniqueTitles = news.DistinctBy(x => x.Title);

            newsWithUniqueTitles = newsWithUniqueTitles.Where( x => !titlesInDb.Contains(x.Title));
            base.AddRange(newsWithUniqueTitles);

            return newsWithUniqueTitles;
        }

        public IEnumerable<string> GetAllTitles()
        {
            return _context.News.Select(x => x.Title).ToList();
        }

        public IEnumerable<News> GetWithPagination(int count, int start)
        {
            return _context.News
                        .OrderByDescending(n => n.Date)
                        .Skip(start)
                        .Take(count)
                        .ToList();
        }
    }
}
