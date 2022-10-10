using Microsoft.EntityFrameworkCore;
using NewsParserApi.Models;

namespace NewsParserApi.Data
{
    public class NewsApiDbContext : DbContext
    {
        public DbSet<News> News { get; set; } = null!;

        public NewsApiDbContext(DbContextOptions<NewsApiDbContext> options) : base(options)
        {

        }
    }
}
