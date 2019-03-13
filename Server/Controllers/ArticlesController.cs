using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Providers;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("Articles")]
    public class ArticlesController : Controller
    {
        private IArticlesProvider articlesProvider;
        private IAuthorizationService authorizationService;

        public ArticlesController(IAuthorizationService authorizationService, IArticlesProvider articlesProvider)
        {
            this.articlesProvider = articlesProvider;
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// Get an article
        /// </summary>
        /// <param name="ArticleName"></param>
        /// <returns>
        /// </returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetArticleAsync(string ArticleName)
        {
            var response = await articlesProvider.GetArticleAsync(ArticleName);

            var authorizationResult = await authorizationService.AuthorizeAsync(User, response.Message.ArticleHeader.Author, "AuthorAuthorization");

            if (authorizationResult.Succeeded)
            {
                if (response.Errors != null && response.Errors.Any())
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        /// <summary>
        /// Posts an article
        /// </summary>
        /// <param name="Overwrite"></param>
        /// <param name="ArticleDTO"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostArticleAsync(bool Overwrite, [FromBody] ArticleDTO ArticleDTO)
        {
            var response = await articlesProvider.PostArticleAsync(Overwrite, ArticleDTO);

            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest();
            }
            return Created(ArticleDTO.ArticleHeader.Name, response);
        }

        /// <summary>
        /// Deletes selected article
        /// </summary>
        /// <param name="ArticleName"></param>
        /// <returns>
        /// </returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(string ArticleName)
        {
            var response = await articlesProvider.DeleteArticleAsync(ArticleName);

            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        /// <summary>
        /// Query list of articles
        /// </summary>
        /// <returns>
        /// List of all created articles
        /// </returns>
        [Route("All")]
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var response = await articlesProvider.GetArticlesListAsync();
            return Ok(response);
        }

        [HttpOptions]
        public void Options()
        {
            Response.Headers.Add("Access-Control-Allow-Headers", "content-type");
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Add("Access-Control-Allow-Methods", "POST");
        }
    }
}