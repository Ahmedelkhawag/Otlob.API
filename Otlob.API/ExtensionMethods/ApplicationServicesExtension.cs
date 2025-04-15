using Microsoft.AspNetCore.Mvc;
using Otlob.API.Errors;
using Otlob.API.Profiles;
using Otlob.Core.Interfaces;
using Otlob.Core.Repositories;
using Otlob.Core.Services;
using Otlob.Repository.Implmentations;
using Otlob.Repository.Repositories;
using Otlob.Service;

namespace Otlob.API.ExtensionMethods
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApllicationService(this IServiceCollection services)
        {
            // Allow Dependency Injection For All Models
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IOrderService), typeof(OrderServiceWithUOF));
            services.AddScoped(typeof(IOrderService), typeof(OrderServiceWithoutUOF));
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
