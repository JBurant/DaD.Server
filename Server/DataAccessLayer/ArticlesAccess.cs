using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.DTO;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ArticleHeader> GetArticleHeaderAsync(string articleName)
        {
            ArticleHeader articleHeader = null;

            using (var context = articleContextFactory.CreateArticleContext())
            {
                var article = await context.Articles.SingleOrDefaultAsync(x => x.Name == articleName);

                if (article != null)
                {
                    articleHeader = Mapper.Map<ArticleHeader>(article);
                }
            }

            return articleHeader;
        }

        //TODO: Updating article vs. new one
        public async Task<bool> WriteArticleAsync(string articleName, string articleAuthor, string articleFile)
        {
            var isSuccesful = false;

            using (var context = articleContextFactory.CreateArticleContext())
            {
                context.Articles.Add(new Article() { Name = articleName, Author = articleAuthor, ArticleContent = articleFile });
                if (await context.SaveChangesAsync(default(CancellationToken)) == 1)
                {
                    isSuccesful = true;
                }
            }

            return isSuccesful;
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

        public async Task<ArticleDTO> GetArticleAsync(string articleName)
        {
            using (var context = articleContextFactory.CreateArticleContext())
            {
                var article = await context.Articles.SingleOrDefaultAsync(x => x.Name == articleName);

                if (article != null)
                {
                    return new ArticleDTO() { ArticleHeader = Mapper.Map<ArticleHeader>(article), ArticleContent = article.ArticleContent };
                }
            }

            return null;
        }

        public async Task<List<ArticleHeader>> GetArticleListAsync()
        {
            var articleList = new List<ArticleHeader>();

            using (var context = articleContextFactory.CreateArticleContext())
            {
                return await context.Articles.Select(x => new ArticleHeader() { Name = x.Name }).ToListAsync();
            }
        }
    }
}