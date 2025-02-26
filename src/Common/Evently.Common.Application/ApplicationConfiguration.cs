using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modulesAssemblies)
        {
            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(modulesAssemblies);
            });
            services.AddValidatorsFromAssemblies(modulesAssemblies, includeInternalTypes: true);
            return services;
        }
    }
}

	
