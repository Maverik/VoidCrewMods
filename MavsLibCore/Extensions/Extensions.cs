namespace MavsLibCore;

public static class Extensions
{
    public static T? As<T>(this object value, T? defaultValue = default) where T : class => value as T ?? defaultValue;

    public static T Cast<T>(this object value) => (T)value;

    public static T BindValue<T>(this ConfigFile config, ConfigDefinition definition, ConfigDescription description) =>
        (description is { Tags.Length: > 0 }
            ? config.Bind(definition, description.Tags.OfType<ConfigurationManagerAttributes>().Select(x => (T)x.DefaultValue).SingleOrDefault(), description).Value
            : default)!;
}