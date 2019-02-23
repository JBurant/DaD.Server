using Common.DTO;
using Identity.DTO;
using System.Threading.Tasks;

namespace Identity.Providers
{
    public interface IUserProvider
    {
        Task<MessageResponse> RegisterUserAsync(RegisterUserRequest message);

        Task<MessageResponse> LoginUserAsync(LoginUserRequest message);
    }
}