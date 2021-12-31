using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            string migrationsAssembly = typeof(Startup).Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(config =>
                                                        {
                                                            config.UseSqlServer(connectionString);
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
            
            services.ConfigureApplicationCookie(config =>
                                                {
                                                    config.Cookie.Name = "Identity.Cookie";
                                                    config.LoginPath = "/Auth/Login";
                                                    config.LogoutPath = "/Auth/Logout";
                                                    config.Cookie.SameSite = SameSiteMode.None;
                                                    config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                                                });

            services.ConfigureNonBreakingSameSiteCookies();

            var assembly = typeof(Startup).Assembly.GetName().Name;
            var filePath = Path.Combine(_environment.ContentRootPath, "stock.pfx");
            var cert = new X509Certificate2(filePath,"password");

            services.AddIdentityServer()
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddConfigurationStore(options =>
                                           {
                                               options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                                                                                                sql => sql.MigrationsAssembly(migrationsAssembly));
                                           })
                    .AddOperationalStore(options =>
                                         {
                                             options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                                                                                              sql => sql.MigrationsAssembly(migrationsAssembly));
                                         })
                    //.AddSigningCredential(cert);
                    //.AddInMemoryApiResources(Configuration.GetApis())
                    //.AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                    //.AddInMemoryClients(Configuration.GetClients())
                    .AddDeveloperSigningCredential();

            //services.AddAuthentication()
            //        .AddFacebook(config =>
            //                     {
            //                         config.AppId = "";
            //                         config.AppSecret = "";
            //                     });

            services.ConfigureCorsPolicy(new List<string>(){ "https://localhost:44321", "http://127.0.0.1:44700" }, _configuration);

            //services.AddCors(options =>
            //                 {
            //                     // this defines a CORS policy called "default"
            //                     options.AddPolicy("default", policy =>
            //                                                  {
            //                                                      policy.WithOrigins("http://localhost:44321")
            //                                                            .AllowAnyHeader()
            //                                                            .AllowAnyMethod();
            //                                                  });
            //                 });
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

            app.UseRouting();

            app.UseStaticFiles();
            app.UseCors("ucwCorsPolicy");
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapDefaultControllerRoute();
                             });
        }
    }
}
