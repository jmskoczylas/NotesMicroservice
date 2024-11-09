using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<INoteRepository, TextFileNoteRepository>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}