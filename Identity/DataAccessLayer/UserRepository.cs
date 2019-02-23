using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Identity.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserRepository(UserManager<IdentityUser> userManager, ApplicationDbContext appDbContext)
        {
            this.userManager = userManager;
        }

        public async Task<string> Create(string username, string email, string password)
        {
            var identityUser = new IdentityUser { UserName = username, Email = email };
            var identityResult = await userManager.CreateAsync(identityUser, password);

            if (!identityResult.Succeeded)
            {
                return null;
            }
            return username;
        }

        public async Task<IdentityUser> FindByName(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<bool> CheckPassword(IdentityUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}