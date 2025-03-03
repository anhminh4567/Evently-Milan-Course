using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Authentication;
internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddAuthentication().AddJwtBearer(opt =>
        {
            // we dont configure stuff here
            // we do it in ConfugreOption ( Options pattern )
        });
        services.AddHttpContextAccessor();
        services.ConfigureOptions<JwtBearerConfigureOptions>();
        return services;
    }
}
