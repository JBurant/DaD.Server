using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.Providers;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    [Route("Posts")]
    public class PostsController : Controller
    {
        private IPostsProvider postsProvider;

        public PostsController(IPostsProvider postsProvider)
        {
            this.postsProvider = postsProvider;
        }

        /// <summary>
        /// Get a post
        /// </summary>
        /// <param name="PostName"></param>
        /// <returns>
        /// </returns>
        [HttpGet]
        public IActionResult GetPost(string PostName)
        {
            var response = postsProvider.GetPost(PostName);

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
        /// Posts a post
        /// </summary>
        /// <param name="PostName"></param>
        /// <param name="Overwrite"></param>
        /// <param name="PostFile"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public IActionResult PostPost(string PostName, bool Overwrite, [FromBody] string PostFile)
        {
            var response = postsProvider.PostPost(PostName, Overwrite, PostFile);

            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest();
            }
            return Created(PostName, response);
        }

        /// <summary>
        /// Deletes selected post
        /// </summary>
        /// <param name="PostName"></param>
        /// <returns>
        /// </returns>
        [HttpDelete]
        public IActionResult DeletePost(string PostName)
        {
            var response = postsProvider.DeletePost(PostName);

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
        /// Query list of posts
        /// </summary>
        /// <returns>
        /// List of all created posts
        /// </returns>
        [Route("All")]
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var response = postsProvider.GetPostsList();
            return Ok(response);
        }
    }
}