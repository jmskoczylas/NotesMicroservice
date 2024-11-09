using Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateNoteValidator>(); // register validators

            return services;
        }
    }
}
