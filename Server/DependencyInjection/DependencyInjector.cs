using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Server.App_Config;
using Server.DataAccessLayer;
using Server.Providers;

namespace Server.DependencyInjection
{
    public class DependencyInjector : IDependencyInjector
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IArticlesProvider, ArticlesProvider>();
            services.AddTransient<IArticlesAccess, ArticlesAccess>();
            services.AddTransient<IDataAccessLayerConfig, DataAccessLayerConfig>();
        }
    }
}