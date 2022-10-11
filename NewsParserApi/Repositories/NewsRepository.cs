using NewsParserApi.Data;
using NewsParserApi.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace NewsParserApi.Repositories
{
    public class NewsRepository : IBaseRepository<News>
    {
        private readonly NewsApiDbContext _context;

        public NewsRepository(NewsApiDbContext context)
        {
            _context = context;
        }

        public void Add(News entity)
        {
            _context.News.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<News> entities)
        {
            _context.News.AddRange(entities);
            _context.SaveChanges();
        }

        public IEnumerable<News> Find(Expression<Func<News, bool>> expression)
        {
            return _context.News.Where(expression);
        }

        public IEnumerable<News> GetAll()
        {
            return _context.News.ToList();
        }

        public News GetById(int id)
        {
            return _context.News.Find(id);
        }

        public void Remove(News entity)
        {
            _context.News.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<News> entities)
        {
            _context.News.RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}
