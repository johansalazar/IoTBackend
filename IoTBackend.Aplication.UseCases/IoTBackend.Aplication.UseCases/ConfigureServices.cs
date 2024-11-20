using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IoTBackend.Aplication.UseCases
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
