using Common.DTO;
using Server.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Providers
{
    public interface IArticlesProvider
    {
        Task<MessageResponse<string>> PostArticleAsync(bool overwrite, ArticleDTO articleDto);

        Task<MessageResponse<string>> DeleteArticleAsync(string articleName);

        Task<MessageResponse<ArticleDTO>> GetArticleAsync(string articleName);

        Task<MessageResponse<List<ArticleHeader>>> GetArticlesListAsync();
    }
}