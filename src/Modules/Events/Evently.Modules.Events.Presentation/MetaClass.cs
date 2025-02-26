global using Evently.Common.Domain; 
using System.Reflection;

namespace Evently.Modules.Events.Presentation;

public static class MetaClass
{
    public static readonly Assembly Assembly = typeof(MetaClass).Assembly;
}
