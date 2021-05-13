using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace Application
{
    /// <summary>
    /// Micro-service entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile($"{path}{"\\configs\\appsettings.json"}")
                .Build();

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureWebHostDefaults(webHostBuilder => { webHostBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    // Add other loggers...
                });
        }
    }
}
