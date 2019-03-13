using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Contexts
{
    public class ArticleDbContext : DbContext
    {
        public virtual DbSet<Article> Articles { get; set; }

        public ArticleDbContext(DbContextOptions<ArticleDbContext> options)
        : base(options)
        {
        }
    }
}