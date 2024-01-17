namespace MavsLibCore;

public interface ISupportsConfigFile
{
    void LoadFrom(ConfigFile config);

    static ConfigDefinition SettingsVersionConfigDefinition => new("Config", nameof(SettingsVersion));

    static ConfigDescription SettingsVersionConfigDescription => new("Internal plugin configuration revision. Do NOT change.", null, new ConfigurationManagerAttributes { ReadOnly = false, IsAdvanced = true });

    ulong SettingsVersion { get; set; }
}