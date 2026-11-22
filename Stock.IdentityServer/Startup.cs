using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stock.IdentityServer.Data;
using Stock.IdentityServer.Extensions;
using Stock.IdentityServer.Extensions.Microsoft.Extensions.DependencyInjection;
using Stock.IdentityServer.Models;
using Config = Microsoft.Extensions.Configuration.IConfiguration;

namespace Stock.IdentityServer
{
    public class Startup
    {
        private readonly Config _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(Config config, IWebHostEnvironment environment)
        {
            _configuration = config;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //string connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(config =>
                                                        {
                                                            //config.UseSqlServer(connectionString);
                                                            config.UseInMemoryDatabase("IdentityServer");
                                                        });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                                                                {
                                                                    config.Password.RequiredLength = 4;
                                                                    config.Password.RequireDigit = false;
                                                                    config.Password.RequireNonAlphanumeric = false;
                                                                    config.Password.RequireUppercase = false;
                                                                })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
            
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddIdentityServer(options =>
                                       {
                                           options.UserInteraction.LoginUrl = "/Auth/Login";
                                           options.UserInteraction.LogoutUrl = "/Auth/Logout";
                                           options.Authentication.CheckSessionCookieSameSiteMode = SameSiteMode.Lax;
                                           //The signing key comes from AddDeveloperSigningCredential below
                                           options.KeyManagement.Enabled = false;
                                       })
                    .AddAspNetIdentity<ApplicationUser>()
                    //To keep the clients, resources and grants in SQL Server instead of in memory, add the
                    //Duende.IdentityServer.EntityFramework package and use AddConfigurationStore/AddOperationalStore here
                    .AddInMemoryApiScopes(Configuration.GetApiScopes())
                    //.AddInMemoryApiResources(Configuration.GetApiResources())
                    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                    .AddInMemoryClients(Configuration.GetClients())
                    .AddDeveloperSigningCredential();

            //This has to come after AddAspNetIdentity, which sets the cookies to SameSite=None.
            //The server runs on plain http for local development and browsers only accept
            //SameSite=None cookies over https. Lax is enough for the redirect based sign in.
            services.ConfigureApplicationCookie(config =>
                                                {
                                                    config.Cookie.Name = "Identity.Cookie";
                                                    config.LoginPath = "/Auth/Login";
                                                    config.LogoutPath = "/Auth/Logout";
                                                    config.Cookie.SameSite = SameSiteMode.Lax;
                                                    config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                                                });
            services.ConfigureExternalCookie(config =>
                                             {
                                                 config.Cookie.SameSite = SameSiteMode.Lax;
                                                 config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                                             });

            //services.AddAuthentication()
            //        .AddFacebook(config =>
            //                     {
            //                         config.AppId = "";
            //                         config.AppSecret = "";
            //                     });

            services.ConfigureCorsPolicy(Configuration.GetClients().ToList(), _configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                SeedData.InitializeDatabase(app.ApplicationServices);
            }

            app.UseCookiePolicy();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("ucwCorsPolicy");
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapDefaultControllerRoute();
                             });
        }
    }
}
