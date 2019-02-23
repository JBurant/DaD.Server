using Common.DTO;
using Server.DTO;

namespace Server.Providers
{
    public interface IArticlesProvider
    {
        MessageResponse PostArticle(bool overwrite, ArticleModel articleFile);

        MessageResponse DeleteArticle(string ArticleName);

        MessageResponse GetArticle(string ArticleName);

        MessageResponse GetArticlesList();
    }
}