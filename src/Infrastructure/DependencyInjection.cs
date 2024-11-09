using Domain.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<INoteRepository, SqlNoteRepository>();
            services.AddAutoMapper(typeof(MapperProfile));

            return services;
        }
    }
}