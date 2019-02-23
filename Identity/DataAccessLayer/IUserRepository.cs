using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Identity.DataAccessLayer
{
    public interface IUserRepository
    {
        Task<string> Create(string userName, string email, string password);

        Task<bool> CheckPassword(IdentityUser user, string password);

        Task<IdentityUser> FindByName(string username);
    }
}