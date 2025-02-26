global using Evently.Common.Domain; 
using System.Reflection;

namespace Evently.Modules.Events.Application;
public static class MetaClass
{
    public static readonly Assembly EventApplicationAssembly = typeof(MetaClass).Assembly;
}
