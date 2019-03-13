using Server.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public interface IArticlesAccess
    {
        Task<ArticleHeader> GetArticleHeaderAsync(string articleName);

        Task<bool> WriteArticleAsync(string articleName, string articleAuthor, string articleFile);

        Task<bool> DeleteArticleAsync(string articleName);

        Task<ArticleDTO> GetArticleAsync(string articleName);

        Task<List<ArticleHeader>> GetArticleListAsync();
    }
}