using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stock.API.Data;

namespace Stock.API.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        //Every factory gets its own database, so test classes do not see each other's changes
        private readonly string _databaseName = "InMemoryDbForTesting-" + Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //Not Development, so the API does not seed its own demo data
            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
                                          {
                                              var descriptors = services.Where(
                                                  d => d.ServiceType == typeof(DbContextOptions<StockDbContext>)
                                                       || d.ServiceType == typeof(IDbContextOptionsConfiguration<StockDbContext>)).ToList();

                                              foreach (var descriptor in descriptors)
                                              {
                                                  services.Remove(descriptor);
                                              }

                                              services.AddDbContext<StockDbContext>(options =>
                                                                                    {
                                                                                        options.UseInMemoryDatabase(_databaseName);
                                                                                    });

                                              //Validate access tokens against the test signing key instead of downloading
                                              //the keys from the identity server
                                              services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                                              {
                                                  options.ConfigurationManager = null;
                                                  options.TokenValidationParameters.ValidIssuer = TestTokens.Issuer;
                                                  options.TokenValidationParameters.IssuerSigningKey = TestTokens.SigningKey;
                                              });
                                          });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<StockDbContext>();
                db.Database.EnsureCreated();
                Utilities.InitializeDbForTests(db);
            }

            return host;
        }
    }
}
