using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MoneyTrackDatabaseAPI.Controllers.Middleware;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.DataAccess;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace BurgerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        bool UseSwagger = true;

        private readonly string AllowedOrigins = "origins";
        private string[] AllowedOriginsAddresses = {"https://localhost:2222"};
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<UserCredentialsDbContext>();
            services.AddScoped<IUserService, UserServiceSqlite>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<ITokenService, TokenServiceJson>();
            services.AddSingleton<SecurityService>();
            services.AddCors(
                options =>
            {
                options.AddPolicy(AllowedOrigins,
                    builder =>
                    {
                        builder.WithOrigins(AllowedOriginsAddresses)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .SetIsOriginAllowedToAllowWildcardSubdomains();;
                    });
            });
            if (UseSwagger)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo {Title = "AuthAPI", Version = "v1"});
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseAuthTokenMiddleware();

            if (UseSwagger)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Burger API v1"));
            }

            app.UseRouting();

            app.UseCors(AllowedOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}