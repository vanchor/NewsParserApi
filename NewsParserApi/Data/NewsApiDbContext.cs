using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NewsParserApi.Models;

namespace NewsParserApi.Data
{
    public class NewsApiDbContext : DbContext
    {
        public DbSet<News> News { get; set; } = null!;

        public NewsApiDbContext(DbContextOptions<NewsApiDbContext> options) : base(options)
        {

            try
            {
                var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if(dbCreator != null)
                {
                    if (!dbCreator.CanConnect()) dbCreator.Create();
                    if (!dbCreator.HasTables()) dbCreator.CreateTables();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
