using Common.DTO;
using Identity.DTO;
using System.Threading.Tasks;

namespace Identity.Providers
{
    public interface IUserProvider
    {
        Task<MessageResponse<string>> RegisterUserAsync(RegisterUserRequest message);

        Task<MessageResponse<string>> LoginUserAsync(LoginUserRequest message);
    }
}