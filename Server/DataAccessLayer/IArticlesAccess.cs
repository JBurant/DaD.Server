using Server.DTO;
using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.DataAccessLayer
{
    public interface IArticlesAccess
    {
        Task WriteArticleAsync(string articleName, string articleAuthor, string articleFile);

        Task<bool> DeleteArticleAsync(string articleName);

        Task<Article> GetArticleAsync(string articleName);

        Task<List<Article>> GetArticleListAsync();
    }
}