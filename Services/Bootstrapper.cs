using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Resolve.Services
{
    public static class Bootstrapper
    {
        public static void AddGraphService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WebOptions>(configuration);
        }
    }
}
