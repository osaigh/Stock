using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Stock.IdentityServer.Controllers;
using Stock.IdentityServer.Models;

namespace Stock.IdentityServer.Data
{
    public class SeedData
    {
        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //test user
                var appDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!appDbContext.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var user = new ApplicationUser()
                               {
                                   UserName = "bob",
                                   FirstName = "Bob",
                                   LastName = "Stack",
                               };
                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();
                    userManager.AddClaimAsync(user, new Claim("meth", "big-things")).GetAwaiter().GetResult();
                    userManager.AddClaimsAsync(user, AuthController.GetNameClaims(user)).GetAwaiter().GetResult();
                }
            }
        }
    }
}
