using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NewsParserApi.Data;
using NewsParserApi.Models;

namespace NewsParserApi.Repositories
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
    }
}
