using Application.Behaviors;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace Application
{
    /// <summary>
    /// Configures application services and the HTTP request pipeline.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration settings.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection used for dependency injection registration.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("LocalFrontend", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins(
                            "http://localhost:5173",
                            "http://127.0.0.1:5173",
                            "http://localhost:8080",
                            "http://127.0.0.1:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddSingleton<INoteRepository>(serviceProvider =>
            {
                var mapper = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
                var logger = serviceProvider.GetRequiredService<ILogger<SqlNoteRepository>>();
                var provider = Configuration["Db:Provider"] ?? "SqlServer";
                var connectionString = Configuration.GetConnectionString("Default")
                    ?? Configuration.GetConnectionString("SqlConnectionString");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Missing connection string. Configure ConnectionStrings:Default or ConnectionStrings:SqlConnectionString.");
                }

                if (!string.Equals(provider, "SqlServer", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(provider, "Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Unsupported Db:Provider. Use SqlServer or Sqlite.");
                }

                return new SqlNoteRepository(mapper, logger, connectionString, provider);
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web host environment.</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("LocalFrontend");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
