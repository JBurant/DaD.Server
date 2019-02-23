using Identity.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public interface IUserController
    {
        Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUserRequest);
    }
}