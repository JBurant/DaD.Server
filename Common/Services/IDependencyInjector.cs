using Microsoft.Extensions.DependencyInjection;

namespace Common.Services
{
    public interface IDependencyInjector
    {
        void RegisterServices(IServiceCollection services);
    }
}