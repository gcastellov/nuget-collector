using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NugetCollector.Api.Configuration;
using NugetCollector.Api.Hubs;
using NugetCollector.Data;
using NugetCollector.Data.Repositories;
using NugetCollector.Domain.Repositories;

namespace NugetCollector.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ProjectContext>();
            services.AddScoped<DbContextOptions<ProjectContext>>();
            services.AddDbContext<ProjectContext>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICodeHubSender, CodeHubSender>();

            var options = _configuration.GetSection("CollectorOptions");
            services.Configure<CollectorOptions>(options);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Nuget Collector API"
                });
            });

            services.AddCors();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nuget Collector API");
            });

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(_configuration["CorsOrigin"]);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CodeHub>("/code-hub");
            });
        }
    }
}
