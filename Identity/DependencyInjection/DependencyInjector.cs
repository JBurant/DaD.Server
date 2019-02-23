using Common.Services;
using Identity.Controllers;
using Identity.DataAccessLayer;
using Identity.Providers;
using Identity.Services;
using Identity.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DependencyInjection
{
    public class DependencyInjector : IDependencyInjector
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserController, UserController>();
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<ITokenFactory, TokenFactory>();
            services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();
            services.AddTransient<IUserValidator, UserValidator>();
        }
    }
}