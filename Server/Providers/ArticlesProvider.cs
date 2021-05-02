using Common.DTO;
using Common.Services;
using Server.DataAccessLayer;
using Server.DTO;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Providers
{
    public class ArticlesProvider : IArticlesProvider
    {
        private IArticlesAccess articlesAccess;
        private IErrorListProvider errorListProvider;

        public ArticlesProvider(IArticlesAccess articlesAccess, IErrorListProvider errorListProvider)
        {
            this.articlesAccess = articlesAccess;
            this.errorListProvider = errorListProvider;
        }

        public async Task<MessageResponse<string>> PostArticleAsync(bool overwrite, ArticleDTO articleDTO)
        {
            var response = new MessageResponse<string>();

            if (await articlesAccess.GetArticleAsync(articleDTO.ArticleHeader.Name) != null)
            {
                if (!overwrite)
                {
                    response.Errors.Add(errorListProvider.GetError(ErrorCode.IE0001));
                    return response;
                }
                if (!await articlesAccess.DeleteArticleAsync(articleDTO.ArticleHeader.Name))
                {
                    response.Errors.Add(errorListProvider.GetError(ErrorCode.IE0010));
                    return response;
                }
            }

            await articlesAccess.WriteArticleAsync(articleDTO.ArticleHeader.Name, articleDTO.ArticleHeader.Author, articleDTO.ArticleContent);
     
            response.Message = articleDTO.ArticleHeader.Name;
            return response;
        }

        public async Task<MessageResponse<string>> DeleteArticleAsync(string articleName)
        {
            var response = new MessageResponse<string>();

            var success = await articlesAccess.DeleteArticleAsync(articleName);

            if (success)
            {
                response.Message = articleName;
            }
            else
            {
                response.Errors.Add(errorListProvider.GetError(ErrorCode.IE0002));
            }

            return response;
        }

        public async Task<MessageResponse<ArticleDTO>> GetArticleAsync(string articleName)
        {
            var response = new MessageResponse<ArticleDTO>();

            var article = await articlesAccess.GetArticleAsync(articleName);

            if (article == null)
            {
                response.Errors.Add(errorListProvider.GetError(ErrorCode.IE0002));
            }
            else
            {
                response.Message = new ArticleDTO() {
                ArticleHeader = new ArticleHeader() {
                    Name = article.Name,
                    Author = article.Author,
                    TimeCreated =article.TimeCreated,
                    TimeModified =article.TimeModified,
                }, ArticleContent = article.ArticleContent};
            }

            return response;
        }

        public async Task<MessageResponse<List<ArticleHeader>>> GetArticlesListAsync()
        {
            var articlesList = await articlesAccess.GetArticleListAsync();

            return new MessageResponse<List<ArticleHeader>>() { Message =  articlesList.Select(x => new ArticleHeader() {
                Name = x.Name,
                Author = x.Author,
                TimeCreated = x.TimeCreated,
                TimeModified = x.TimeModified}).ToList() };
        }
    }
}