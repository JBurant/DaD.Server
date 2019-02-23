using System.Threading.Tasks;

namespace Identity.Services
{
    public interface IJwtTokenFactory
    {
        Task<string> GenerateEncodedToken(string id, string userName);
    }
}