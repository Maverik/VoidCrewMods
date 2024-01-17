namespace BetterShields;

sealed class PluginConfiguration() : ISupportsConfigFile
{
    public void LoadFrom(ConfigFile config)
    {
        HitPoints = config.Bind(MavsDefaults.ConfigSectionName, nameof(HitPoints), 25f,
                new ConfigDescription("Determines how much total damage can the shield absorb. Game default is 10f",
                    new AcceptableValueRange<float>(10f, 50f)))
            .Value;
        AbsorptionRate = config.Bind(MavsDefaults.ConfigSectionName, nameof(AbsorptionRate), 1f,
                new ConfigDescription("Determines how long it takes before shields start recovering. Game default is 0.5f",
                    new AcceptableValueRange<float>(0f, 1f)))
            .Value;
        RechargeDelay = config.Bind(MavsDefaults.ConfigSectionName, nameof(RechargeDelay), 7.5f,
                new ConfigDescription("Determines how long it takes before shields start recovering. Game default is 5f",
                    new AcceptableValueRange<float>(3f, 10f)))
            .Value;
        RechargeSpeed = config.Bind(MavsDefaults.ConfigSectionName, nameof(RechargeSpeed), 1.5f,
                new ConfigDescription("Determines how quickly the shield hp is replenished. Game default is 1f",
                    new AcceptableValueRange<float>(1f, 5f)))
            .Value;
    }

    public ulong SettingsVersion { get; set; }

    public float AbsorptionRate { get; set; } = 0.5f;

    public float RechargeSpeed { get; set; } = 1f;

    public float RechargeDelay { get; set; } = 5f;

    public float HitPoints { get; set; } = 10f;
}