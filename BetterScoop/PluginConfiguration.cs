using BepInEx.Configuration;

namespace BetterScoop;

[PublicAPI]
public struct PluginConfiguration : IEquatable<PluginConfiguration>
{
    const string _configSection = "Config";

    public PluginConfiguration(ConfigFile config)
    {
        _ = config ?? throw new ArgumentNullException(nameof(config));

        ItemSearchInterval = config.Bind(_configSection, nameof(ItemSearchInterval), 0.1f,
            new ConfigDescription("Determines how frequently scoop checks for loot per second. Game default interval is 0.25 seconds",
                new AcceptableValueRange<float>(0.1f, 0.25f)));
        ItemSearchInitialDelay = config.Bind(_configSection, nameof(ItemSearchInitialDelay), 0.1f,
            new ConfigDescription("Determines how quickly the scoop starts working once it gets turned on. Game default delay is 0.5 seconds",
                new AcceptableValueRange<float>(0.1f, 0.5f)));
        ConeAngle = config.Bind(_configSection, nameof(ConeAngle), 165f,
            new ConfigDescription("Determines how wide the cone that extends to the max range is. This is the maximum cone angle and will get restricted by collision with things like the hull or thrusters blocking line of sight. Game default is 90 degrees.",
                new AcceptableValueRange<float>(90f, 180f)));
        MaxRange = config.Bind(_configSection, nameof(MaxRange), 750f,
            new ConfigDescription("Determines maximum range of the scoop in meters. Game default is 200 meters.",
                new AcceptableValueRange<float>(200f, 1000f)));
        PullVelocity = config.Bind(_configSection, nameof(PullVelocity), 150f,
            new ConfigDescription("Determines how quickly the items travel to the scoop once linked. Game default is 15 meters per second",
                new AcceptableValueRange<float>(15f, 250f)));
        CatchRadius = config.Bind(_configSection, nameof(CatchRadius), 9f,
            new ConfigDescription("Determines when the item is considered to be close enough to just dock into the scoop. Game default is 3 meters.",
                new AcceptableValueRange<float>(3f, 12f)));
        InheritVelocityValue = config.Bind(_configSection, nameof(InheritVelocityValue), 0.01f,
            new ConfigDescription("Determines how much of the ship velocity is transferred to loot before scoop starts accelerating it towards itself. Game default is 0.85 or 85%.",
                new AcceptableValueRange<float>(0.01f, 1f)));
    }

    public ConfigEntry<float> ItemSearchInterval { get; set; }

    public ConfigEntry<float> ItemSearchInitialDelay { get; set; }

    public ConfigEntry<float> ConeAngle { get; set; }

    public ConfigEntry<float> MaxRange { get; set; }

    public ConfigEntry<float> PullVelocity { get; set; }

    public ConfigEntry<float> CatchRadius { get; set; }

    public ConfigEntry<float> InheritVelocityValue { get; set; }

    public bool Equals(PluginConfiguration other) => ItemSearchInterval.Equals(other.ItemSearchInterval) && ItemSearchInitialDelay.Equals(other.ItemSearchInitialDelay) && ConeAngle.Equals(other.ConeAngle) && MaxRange.Equals(other.MaxRange) && PullVelocity.Equals(other.PullVelocity) && CatchRadius.Equals(other.CatchRadius) &&
                                                     InheritVelocityValue.Equals(other.InheritVelocityValue);

    public override bool Equals(object? obj) => obj is PluginConfiguration other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(ItemSearchInterval, ItemSearchInitialDelay, ConeAngle, MaxRange, PullVelocity, CatchRadius, InheritVelocityValue);

    public static bool operator ==(PluginConfiguration left, PluginConfiguration right) => left.Equals(right);

    public static bool operator !=(PluginConfiguration left, PluginConfiguration right) => !left.Equals(right);
}