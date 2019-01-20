using Server.DTO;
using System.Collections.Generic;

namespace Server.DataAccessLayer
{
    public interface IArticlesAccess
    {
        bool FileExists(string articleName);

        bool WriteArticle(string articleName, string articleAuthor, string articleFile);

        void DeleteArticle(string articleName);

        ArticleModel GetArticle(string articleName);

        List<ArticleHeader> GetArticleList();
    }
}