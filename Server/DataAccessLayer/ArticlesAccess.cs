using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public class ArticlesAccess : IArticlesAccess
    {
        private readonly IArticleContextFactory articleContextFactory;

        public ArticlesAccess(IArticleContextFactory articleContextFactory)
        {
            this.articleContextFactory = articleContextFactory;
        }

        //TODO: Updating article vs. new one
        public async Task WriteArticleAsync(string articleName, string articleAuthor, string articleFile)
        {
            using (var context = articleContextFactory.CreateArticleContext())
            {
                context.Articles.Add(new Article() { Name = articleName, Author = articleAuthor, ArticleContent = articleFile });
                await context.SaveChangesAsync(default(CancellationToken));
            }
        }

        public async Task<bool> DeleteArticleAsync(string articleName)
        {
            using (var context = articleContextFactory.CreateArticleContext())
            {
                var article = await context.Articles.SingleOrDefaultAsync(x => x.Name == articleName);

                if (article != null)
                {
                    context.Articles.Remove(article);

                    if (await context.SaveChangesAsync(default(CancellationToken)) == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<Article> GetArticleAsync(string articleName)
        {
            using (var context = articleContextFactory.CreateArticleContext())
            {
                var article = await context.Articles.SingleOrDefaultAsync(x => x.Name == articleName);
                return article;
            }
        }

        public async Task<List<Article>> GetArticleListAsync()
        {
            using (var context = articleContextFactory.CreateArticleContext())
            {
                return await context.Articles.ToListAsync();
            }
        }
    }
}