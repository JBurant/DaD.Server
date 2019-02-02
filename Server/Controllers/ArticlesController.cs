using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Providers;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    [Route("Articles")]
    public class ArticlesController : Controller
    {
        private IArticlesProvider articlesProvider;

        public ArticlesController(IArticlesProvider articlesProvider)
        {
            this.articlesProvider = articlesProvider;
        }

        /// <summary>
        /// Get an article
        /// </summary>
        /// <param name="ArticleName"></param>
        /// <returns>
        /// </returns>
        [HttpGet]
        public IActionResult GetArticle(string ArticleName)
        {
            var response = articlesProvider.GetArticle(ArticleName);

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
        /// Posts an article
        /// </summary>
        /// <param name="Overwrite"></param>
        /// <param name="ArticleModel"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public IActionResult PostArticle(bool Overwrite, [FromBody] ArticleModel ArticleModel)
        {
            var response = articlesProvider.PostArticle(Overwrite, ArticleModel);

            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest();
            }
            return Created(ArticleModel.ArticleHeader.Name, response);
        }

        /// <summary>
        /// Deletes selected article
        /// </summary>
        /// <param name="ArticleName"></param>
        /// <returns>
        /// </returns>
        [HttpDelete]
        public IActionResult DeleteArticle(string ArticleName)
        {
            var response = articlesProvider.DeleteArticle(ArticleName);

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
        public IActionResult GetAllArticles()
        {
            var response = articlesProvider.GetArticlesList();
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