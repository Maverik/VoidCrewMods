namespace BetterScoop;

sealed class PluginConfiguration() : ISupportsConfigFile
{
    public void LoadFrom(ConfigFile config)
    {
        MaxRange = config.Bind(MavsDefaults.ConfigSectionName, nameof(MaxRange), 750f,
                new ConfigDescription("Determines maximum range of the scoop in meters. Game default is 200 meters.",
                    new AcceptableValueRange<float>(200f, 1000f)))
            .Value;

        ItemSearchInterval = config.Bind(MavsDefaults.ConfigSectionName, nameof(ItemSearchInterval), 0.1f,
                new ConfigDescription("Determines how frequently scoop checks for loot per second. Game default interval is 0.25 seconds",
                    new AcceptableValueRange<float>(0.1f, 0.25f), new ConfigurationManagerAttributes { IsAdvanced = true }))
            .Value;

        ItemSearchInitialDelay = config.Bind(MavsDefaults.ConfigSectionName, nameof(ItemSearchInitialDelay), 0.1f,
                new ConfigDescription("Determines how quickly the scoop starts working once it gets turned on. Game default delay is 0.5 seconds",
                    new AcceptableValueRange<float>(0.1f, 0.5f), new ConfigurationManagerAttributes { IsAdvanced = true }))
            .Value;

        ConeAngle = config.Bind(MavsDefaults.ConfigSectionName, nameof(ConeAngle), 165f,
                new ConfigDescription("Determines how wide the cone that extends to the max range is. This is the maximum cone angle and will get restricted by collision with things like the hull or thrusters blocking line of sight. Game default is 90 degrees.",
                    new AcceptableValueRange<float>(90f, 180f), new ConfigurationManagerAttributes { IsAdvanced = true }))
            .Value;

        PullVelocity = config.Bind(MavsDefaults.ConfigSectionName, nameof(PullVelocity), 150f,
                new ConfigDescription("Determines how quickly the items travel to the scoop once linked. Game default is 15 meters per second",
                    new AcceptableValueRange<float>(15f, 250f), new ConfigurationManagerAttributes { IsAdvanced = true }))
            .Value;

        InheritVelocityValue = config.Bind(MavsDefaults.ConfigSectionName, nameof(InheritVelocityValue), 0.01f,
                new ConfigDescription("Determines how much of the ship velocity is transferred to loot before scoop starts accelerating it towards itself. Game default is 0.85 or 85%.",
                    new AcceptableValueRange<float>(0.01f, 1f), new ConfigurationManagerAttributes { IsAdvanced = true }))
            .Value;

        CatchRadius = config.Bind(MavsDefaults.ConfigSectionName, nameof(CatchRadius), 9f,
                new ConfigDescription("Determines when the item is considered to be close enough to just dock into the scoop. Game default is 3 meters.",
                    new AcceptableValueRange<float>(3f, 12f), new ConfigurationManagerAttributes { IsAdvanced = true, ReadOnly = true }))
            .Value;

        PullMissionItems = config.Bind(MavsDefaults.ConfigSectionName, nameof(PullMissionItems), false, "This setting controls if the scoop should pull mission items like power fuses & oxygen tank").Value;
    }

    public bool PullMissionItems { get; set; }

    public float ItemSearchInterval { get; set; } = 0.25f;

    public float ItemSearchInitialDelay { get; set; } = 0.5f;

    public float ConeAngle { get; set; } = 90f;

    public float MaxRange { get; set; } = 200f;

    public float PullVelocity { get; set; } = 15f;

    public float CatchRadius { get; set; } = 3f;

    public float InheritVelocityValue { get; set; } = 0.85f;
}