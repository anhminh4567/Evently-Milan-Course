using Newtonsoft.Json;

namespace Evently.Common.Infrastructure.Serialization;

public static class SerializerSettings
{
    public static readonly JsonSerializerSettings Instance = new()
    {
        TypeNameHandling = TypeNameHandling.All, // automaticly include the type of the serialized object 
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };
}
