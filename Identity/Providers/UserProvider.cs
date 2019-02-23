using Common.DTO;
using Identity.DataAccessLayer;
using Identity.DTO;
using Identity.Services;
using Identity.Validators;
using System.Threading.Tasks;

namespace Identity.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenFactory jwtFactory;
        private readonly ITokenFactory tokenFactory;
        private readonly IUserValidator userValidator;

        public UserProvider(IUserRepository userRepository, IJwtTokenFactory jwtFactory, ITokenFactory tokenFactory, IUserValidator userValidator)
        {
            this.userRepository = userRepository;
            this.jwtFactory = jwtFactory;
            this.tokenFactory = tokenFactory;
            this.userValidator = userValidator;
        }

        public async Task<MessageResponse> RegisterUserAsync(RegisterUserRequest message)
        {
            var response = new MessageResponse();

            if (string.IsNullOrEmpty(message.Username) || !userValidator.ValidateUsername(message.Username))
            {
                response.Errors.Add(new Error(ErrorCode.IE0007));
                return response;
            }

            if (string.IsNullOrEmpty(message.Password))
            {
                response.Errors.Add(new Error(ErrorCode.IE0008));
                return response;
            }

            if (string.IsNullOrEmpty(message.Email) || !userValidator.ValidateUsername(message.Username))
            {
                response.Errors.Add(new Error(ErrorCode.IE0009));
                return response;
            }

            var repositoryResponse = await userRepository.Create(message.Username, message.Email, message.Password);

            if (repositoryResponse != message.Username)
            {
                response.Errors.Add(new Error(ErrorCode.IE0003));
            }
            else
            {
                response.Message = message.Username;
            }

            return response;
        }

        public async Task<MessageResponse> LoginUserAsync(LoginUserRequest message)
        {
            var response = new MessageResponse();

            if (string.IsNullOrEmpty(message.Username) || !userValidator.ValidateUsername(message.Username))
            {
                response.Errors.Add(new Error(ErrorCode.IE0007));
                return response;
            }

            if (string.IsNullOrEmpty(message.Password))
            {
                response.Errors.Add(new Error(ErrorCode.IE0008));
                return response;
            }

            var user = await userRepository.FindByName(message.Username);
            if (user == null)
            {
                response.Errors.Add(new Error(ErrorCode.IE0005));
                return response;
            }

            if (await userRepository.CheckPassword(user, message.Password))
            {
                response.Message = await jwtFactory.GenerateEncodedToken(user.Id, user.UserName);
            }
            else
            {
                response.Errors.Add(new Error(ErrorCode.IE0004));
            }

            return response;
        }
    }
}