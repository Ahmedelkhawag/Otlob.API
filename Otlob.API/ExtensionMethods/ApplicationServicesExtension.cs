using Microsoft.AspNetCore.Mvc;
using Otlob.API.Errors;
using Otlob.API.Profiles;
using Otlob.Core.Repositories;
using Otlob.Repository.Repositories;

namespace Otlob.API.ExtensionMethods
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApllicationService(this IServiceCollection services)
        {
            // Allow Dependency Injection For All Models
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            // Add AutoMapper Service
            services.AddAutoMapper(typeof(MappingProfiles));
            // Handling Validation Error
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors)
                    .Select(em => em.ErrorMessage)
                    .ToArray();

                    var validationErrorResponse = new ValidationErrorResponse() { Errors = errors };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return services;
        }
    }
}
