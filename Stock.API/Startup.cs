using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Stock.API.Data;
using Stock.API.Repository;
using Stock.API.Models;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Stock.API.Errors;
using Stock.API.Services;
using IdentityServer4.AccessTokenValidation;

namespace Stock.API
{
    public class Startup
    {
        public IConfiguration _Configuration { get; }
        public IWebHostEnvironment _WebHostEnvironment { get; }

        public Startup(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _Configuration = configuration;
            _WebHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Database
            string connectionString = _Configuration.GetConnectionString("DefaultConnection");

            string identityServerUrl = _Configuration.GetValue(typeof(string), "IdentityServerUrl").ToString();
            string apiScope = _Configuration.GetValue(typeof(string), "APIScope").ToString();

            //To run Stock.API.Test comeent out lines 47 - 64 and comment out the Authorization headers on the controllers
            services.AddAuthentication(options => options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer("Bearer",
                                  config =>
                                  {
                                      config.Authority = identityServerUrl;
                                      config.Audience = apiScope;
                                      config.RequireHttpsMetadata = false;
                                      config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                                      {
                                          ValidateAudience = false,

                                      };
                                  });

            services.AddAuthorization(options =>
                                      {
                                          options.AddPolicy("StockAPIPolicy",
                                                            policy =>
                                                            {
                                                                policy.AuthenticationSchemes.Add("Bearer");
                                                                policy.AddRequirements(new Stock.API.Authorization.StockAPIRequirement(apiScope));
                                                            });

                                      });

            services.AddScoped<IAuthorizationHandler, Stock.API.Authorization.StockAPIRequirementHandler>();
            services.AddCors(config =>
                             {
                                 config.AddPolicy("AllowAll",
                                                  p =>
                                                  {
                                                      p.AllowAnyOrigin();
                                                      p.AllowAnyHeader();
                                                      p.AllowAnyMethod();

                                                  });
                             });

            //if (_WebHostEnvironment.IsDevelopment())
            //{
            //    if (connectionString.Contains("%CONTENTROOTPATH%"))
            //    {
            //        connectionString = connectionString.Replace("%CONTENTROOTPATH%", _WebHostEnvironment.ContentRootPath);
            //    }
            //}
            //services.AddDbContext<StockDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<StockDbContext>(options => options.UseInMemoryDatabase("InMemoryDbFor"));

            //AutoMapper
            var mappingConfiguration = new MapperConfiguration(config =>
                                                               {
                                                                   config.AddProfile(new MappingProfile());
                                                               });
            IMapper mapper = mappingConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockAPI", Version = "v1" });
            });


            //StockMarketService
            services.AddScoped<IStockMarketService, StockMarketService>();

            //Repositories
            services.AddScoped<IRepository<StockHolder>, StockHolderRepository>();
            services.AddScoped<IRepository<StockHolderPosition>, StockHolderPositionRepository>();
            services.AddScoped<IRepository<Stock.API.Models.Stock>, StockRepository>();
            services.AddScoped<IRepository<Stock.API.Models.StockHistory>, StockHistoryRepository>();
            services.AddScoped<IRepository<StockPrice>, StockPriceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StockDbContext stockDbContext)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                SeedData.InitializeDB(stockDbContext);
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
            });

            app.UseExceptionHandler(appError =>
                                    {
                                        appError.Run(async context =>
                                                     {
                                                         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                                         context.Response.ContentType = "application/json";
                                                         IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                                                         if (contextFeature != null)
                                                         {

                                                             var errorMessage = new ErrorMessage
                                                                                {
                                                                                    Message = (contextFeature.Error != null) ? contextFeature.Error.Message : "Internal Server Error",
                                                                                    StackTrace = (contextFeature.Error != null) ? contextFeature.Error.StackTrace : string.Empty
                                                                                };

                                                             var jsonString = JsonConvert.SerializeObject(errorMessage);

                                                             await context.Response.WriteAsync(jsonString);
                                                         }
                                                     });
                                    });

            //app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
