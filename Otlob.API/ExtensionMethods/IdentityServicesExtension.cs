using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Otlob.Core.Models;
using Otlob.Core.Services;
using Otlob.Repository.Identity;
using Otlob.Service;

namespace Otlob.API.ExtensionMethods
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenServices, TokenServices>();

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(); // AddJwtBearer => Will Validate Token

            return services;
        }
    }
}
