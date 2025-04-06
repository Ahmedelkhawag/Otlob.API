using Microsoft.EntityFrameworkCore;
using Otlob.Repository.Data;
using Otlob.Repository.Identity;

namespace Otlob.API.ExtensionMethods
{
    public static class ConfigureDatabasesExtension
    {
        public static IServiceCollection AddConnectionStrings(this IServiceCollection services, IConfiguration configuration)
        {
            // Connection String (Main Database)
            ConfigureConnectionString<API1DbContext>(services, configuration);

            // Connection String (Identity)
            ConfigureConnectionString<ApplicationIdentityDbContext>(services, configuration);
            return services;
        }

        private static IServiceCollection ConfigureConnectionString<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
        { 
        var ConnectionString = typeof(T) == typeof(API1DbContext) ? "DefaultConnection" : "IdentityConnection";

            services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionString));
            });
            return services;
        }
    }
}
