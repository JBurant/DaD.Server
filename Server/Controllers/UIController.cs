using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class UIController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult GetIndex()
        {
            return View("IndexView");
        }
    }
}