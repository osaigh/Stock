using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Stock.API.Data;
using Stock.API.Errors;
using Stock.API.Models;
using Stock.API.Repository;
using Stock.API.Services;

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

            string identityServerUrl = _Configuration.GetValue<string>("IdentityServerUrl");
            string apiScope = _Configuration.GetValue<string>("APIScope");

            services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                                  config =>
                                  {
                                      config.Authority = identityServerUrl;
                                      //The identity server runs on plain http for local development
                                      config.RequireHttpsMetadata = false;
                                      config.TokenValidationParameters = new TokenValidationParameters()
                                      {
                                          //The identity server issues tokens without an audience; access is checked by scope instead
                                          ValidateAudience = false,
                                          ValidTypes = new[] { "at+jwt" },
                                      };
                                  });

            services.AddAuthorization(options =>
                                      {
                                          options.AddPolicy("StockAPIPolicy",
                                                            policy =>
                                                            {
                                                                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
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

            //Database. The data lives in memory and is re-seeded every time the API starts.
            //string connectionString = _Configuration.GetConnectionString("DefaultConnection");
            //if (_WebHostEnvironment.IsDevelopment())
            //{
            //    if (connectionString.Contains("%CONTENTROOTPATH%"))
            //    {
            //        connectionString = connectionString.Replace("%CONTENTROOTPATH%", _WebHostEnvironment.ContentRootPath);
            //    }
            //}
            //services.AddDbContext<StockDbContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<StockDbContext>(options => options.UseInMemoryDatabase("InMemoryDbFor"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT access token issued by Stock.IdentityServer. Enter the token only, without the 'Bearer' prefix.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });


            //StockMarketService
            services.AddScoped<IStockMarketService, StockMarketService>();

            //Repositories
            services.AddScoped<IRepository<StockOrder>, StockOrderRepository>();
            services.AddScoped<IRepository<StockPosition>, StockPositionRepository>();
            services.AddScoped<IRepository<Stock.API.Models.Stock>, StockRepository>();
            services.AddScoped<IRepository<Stock.API.Models.StockHistory>, StockHistoryRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StockDbContext stockDbContext)
        {
            if (env.IsDevelopment())
            {
                SeedData.InitializeDB(stockDbContext);
            }

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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
            });

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
