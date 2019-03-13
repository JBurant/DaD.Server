using Common.DTO;
using Common.Services;
using Server.DataAccessLayer;
using Server.DTO;
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

            if (!await articlesAccess.WriteArticleAsync(articleDTO.ArticleHeader.Name, articleDTO.ArticleHeader.Author, articleDTO.ArticleContent))
            {
                response.Errors.Add(errorListProvider.GetError(ErrorCode.IE0011));
                return response;
            }

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
                response.Message = article;
            }

            return response;
        }

        public async Task<MessageResponse<List<ArticleHeader>>> GetArticlesListAsync()
        {
            return new MessageResponse<List<ArticleHeader>>() { Message = await articlesAccess.GetArticleListAsync() };
        }
    }
}