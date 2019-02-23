using Common.DTO;
using Newtonsoft.Json;
using Server.DataAccessLayer;
using Server.DTO;

namespace Server.Providers
{
    public class ArticlesProvider : IArticlesProvider
    {
        private IArticlesAccess articlesAccess;

        public ArticlesProvider(IArticlesAccess articlesAccess)
        {
            this.articlesAccess = articlesAccess;
        }

        public MessageResponse PostArticle(bool overwrite, ArticleModel articleModel)
        {
            var response = new MessageResponse();

            if (!overwrite && articlesAccess.FileExists(articleModel.ArticleHeader.Name))
            {
                response.Errors.Add(new Error(ErrorCode.IE0001));
                return response;
            }

            articlesAccess.WriteArticle(articleModel.ArticleHeader.Name, articleModel.ArticleHeader.Author, articleModel.ArticleContent);
            response.Message = articleModel.ArticleHeader.Name;

            return response;
        }

        public MessageResponse DeleteArticle(string articleName)
        {
            var response = new MessageResponse();

            if (articlesAccess.FileExists(articleName))
            {
                articlesAccess.DeleteArticle(articleName);
                response.Message = articleName;
            }
            else
            {
                response.Errors.Add(new Error(ErrorCode.IE0002));
            }

            return response;
        }

        public MessageResponse GetArticle(string articleName)
        {
            var response = new MessageResponse();

            if (articlesAccess.FileExists(articleName))
            {
                var articleModel = articlesAccess.GetArticle(articleName);
                response.Message = JsonConvert.SerializeObject(articleModel);
            }
            else
            {
                response.Errors.Add(new Error(ErrorCode.IE0002));
            }

            return response;
        }

        public MessageResponse GetArticlesList()
        {
            return new MessageResponse() { Message = JsonConvert.SerializeObject(articlesAccess.GetArticleList()) };
        }
    }
}