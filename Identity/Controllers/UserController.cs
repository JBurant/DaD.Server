using Identity.DTO;
using Identity.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : Controller, IUserController
    {
        private IUserProvider userProvider;

        public UserController(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            var response = await userProvider.RegisterUserAsync(registerUserRequest);

            if (response.Errors != null && response.Errors.Any())
            {
                return BadRequest(response);
            }

            return Created(registerUserRequest.Username, response);
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <returns>
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest request)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            var response = await userProvider.LoginUserAsync(request);
            if (!ModelState.IsValid) { BadRequest(response); }

            return Created(request.Username, response);
        }
    }
}