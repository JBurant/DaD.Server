using Microsoft.EntityFrameworkCore;

namespace Server.Contexts
{
    public class ArticleContextFactory : IArticleContextFactory
    {
        private readonly string databaseConnection;

        public ArticleContextFactory(string databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public ArticleDbContext CreateArticleContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            optionsBuilder.UseSqlServer(databaseConnection);

            return new ArticleDbContext(optionsBuilder.Options);
        }
    }
}