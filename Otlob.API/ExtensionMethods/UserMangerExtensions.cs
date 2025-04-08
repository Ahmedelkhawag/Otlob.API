using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Otlob.Core.Models;
using System.Security.Claims;

namespace Otlob.API.ExtensionMethods
{
    public static class UserMangerExtensions
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal principal)
        { 
        var email = principal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
