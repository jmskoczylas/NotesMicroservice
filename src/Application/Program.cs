using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Application
{
    /// <summary>
    /// Micro-service entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        /// <summary>
        /// Creates and configures the host builder for the web application.
        /// </summary>
        /// <returns>A configured <see cref="IHostBuilder"/> instance.</returns>
        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder.Sources.Clear();
                    var environment = hostingContext.HostingEnvironment.EnvironmentName;

                    builder.SetBasePath(AppContext.BaseDirectory);
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    builder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webHostBuilder => { webHostBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    // Add other loggers...
                });
        }
    }
}
