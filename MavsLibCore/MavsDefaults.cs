using Newtonsoft.Json.Serialization;

namespace MavsLibCore;

public static class MavsDefaults
{
    public const string ConfigSectionName = "Config";
    
    public static JsonSerializerSettings DefaultJsonSerializerOptions { get; } = new()
    {
        EqualityComparer = StructuralComparisons.StructuralEqualityComparer,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
}