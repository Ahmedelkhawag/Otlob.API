
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Otlob.API.Errors;
using Otlob.API.ExtensionMethods;
using Otlob.API.Middlewares;
using Otlob.API.Profiles;
using Otlob.Core.Models;
using Otlob.Core.Repositories;
using Otlob.Repository;
using Otlob.Repository.Data;
using Otlob.Repository.Identity;
using Otlob.Repository.Repositories;
using System.Threading.Tasks;

namespace Otlob.API
{
    public class Program
    {
        public static  async Task Main(string[] args)
        {
            #region Create Host
            var builder = WebApplication.CreateBuilder(args);
            #endregion

            #region ConfigureServices
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Connection String
            // To Use AddDbContext<>() => Add reference from Talabat.Repository Layer
            //builder.Services.AddDbContext<API1DbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});
            #endregion

            #region Extension Methods
            // Configure Entity Framework Core
            builder.Services.AddConnectionStrings(builder.Configuration);

            // Configure Redis
            builder.Services.AddRedisConnectionString(builder.Configuration);

            // Configure the Application Services
            builder.Services.AddApllicationService();

            // Configure Identity Services
            builder.Services.AddIdentityServices(builder.Configuration);
            #endregion

            #region Build Project
            var app = builder.Build();
            #endregion

            // Configure the HTTP request pipeline.
            #region Midelwares
            // Configure the HTTP request pipeline.

            #region Data seed service
            // 1-Group Of Services Lifetime Scoped
            using var scope = app.Services.CreateScope();
            // 2-Services Self
            var services = scope.ServiceProvider;
            // 3-Ask Clr For Creating Object From DbContext Explicitly
            var dbContext = services.GetRequiredService<API1DbContext>();

            var IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
            // Seeding Data
            DataSeedInitializer.SedDataAsync(dbContext);
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            await IdentityDataSeedInitializer.SeedUSerAsynd(userManager);
            #endregion
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.MapOpenApi();
                app.UseSwaggerMiddleware();
            }
            app.UseStatusCodePagesWithRedirects("errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();
            #endregion
            app.Run();
        }
    }
}
