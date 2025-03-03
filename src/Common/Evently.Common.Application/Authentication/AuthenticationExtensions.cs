using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Application.Authentication;
internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        return services;
    }
}
