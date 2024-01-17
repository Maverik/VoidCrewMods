namespace BetterScanner;

sealed class PluginConfiguration : ISupportsConfigFile
{
    public void LoadFrom(ConfigFile config)
    {
        HelmScanningRange = config.Bind(MavsDefaults.ConfigSectionName, nameof(HelmScanningRange), 10000f,
                new ConfigDescription("Determines how far can we scan a target from. Game default is 3000m",
                    new AcceptableValueRange<float>(3000f, 12000f)))
            .Value;

        PassiveScanningRange = config.Bind(MavsDefaults.ConfigSectionName, nameof(PassiveScanningRange), 750f,
                new ConfigDescription("Determines how far the ship can scan targets around it. Game default is 150m",
                    new AcceptableValueRange<float>(150f, 1000f)))
            .Value;
    }

    public ulong SettingsVersion { get; set; }

    public float HelmScanningRange { get; set; } = 3000f;

    public float PassiveScanningRange { get; set; } = 150f;
}