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

        public void Add(News entity)
        {
            if (_context.News.Any(n => n.Title == entity.Title))
                throw new ArgumentException("A news item with this title already exists.");
            base.Add(entity);
        }

        public IEnumerable<News> GetWithPagination(int count, int page)
        {
            int skipCount = count * page;
            return _context.News
                        .OrderByDescending(n => n.Date)
                        .Skip(skipCount)
                        .Take(count)
                        .ToList();
        }
    }
}
