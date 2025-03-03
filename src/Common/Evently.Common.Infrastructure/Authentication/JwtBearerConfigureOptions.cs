using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Evently.Common.Infrastructure.Authentication;
internal class JwtBearerConfigureOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly IConfiguration _configuration;
    private const string ConfigurationSectionName = "Authentication";

    public JwtBearerConfigureOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
