global using Evently.Common.Domain;
global using Evently.Common.Application.Data;
using System.Reflection;

namespace Evently.Modules.Events.Application;
public static class MetaClass
{
    public static readonly Assembly EventApplicationAssembly = typeof(MetaClass).Assembly;
}
