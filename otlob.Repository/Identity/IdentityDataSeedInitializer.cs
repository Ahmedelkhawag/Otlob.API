using Microsoft.AspNetCore.Identity;
using Otlob.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Identity
{
    public static class IdentityDataSeedInitializer
    {

        public static async Task SeedUSerAsynd(UserManager<AppUser> userManager)
        {
            if (!(userManager.Users.Any()))
            {
                AppUser user = new AppUser
                {
                    DisplayName = "Abdalrhman Gamal",
                    Email = "AbdalrhmanGamal681@gmail.com",
                    UserName = "AbdalrhmanGamal681",
                    PhoneNumber = "01067377533",
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
