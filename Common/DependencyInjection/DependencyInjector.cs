using Common.DTO;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection
{
    public class DependencyInjector : IDependencyInjector
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IErrorListProvider, ErrorListProvider>();
        }
    }
}