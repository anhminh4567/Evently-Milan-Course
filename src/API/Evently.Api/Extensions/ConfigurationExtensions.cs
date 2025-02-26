namespace Evently.Api.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddModulesAppsettings(this IConfigurationBuilder configuration, string[] modulesName)
    {
        foreach (var name in modulesName) 
        {
            configuration.AddJsonFile($"modules.{name}.json", false, true);
            configuration.AddJsonFile($"modules.{name}.Development.json", false, true);
        }
        return configuration;
    }
}
