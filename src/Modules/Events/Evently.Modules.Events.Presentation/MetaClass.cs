global using Evently.Common.Domain;
global using Evently.Common.Presentation.ApiResults;
global using Evently.Common.Presentation.Endpoints;

using System.Reflection;

namespace Evently.Modules.Events.Presentation;

public static class MetaClass
{
    public static readonly Assembly Assembly = typeof(MetaClass).Assembly;
}
