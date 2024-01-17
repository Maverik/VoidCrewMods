using Gameplay.MissionDifficulty;

namespace HigherDifficulty;

sealed class PluginConfiguration : ISupportsConfigFile
{
    public void LoadFrom(ConfigFile config)
    {
        Normal.EnemyHpMultiplier = config.BindValue<float>(ConfigurationDefaults.Normal.EnemyHpMultiplier.Key, ConfigurationDefaults.Normal.EnemyHpMultiplier.Value);
        Normal.PlayerShipDamageMultiplier = config.BindValue<float>(ConfigurationDefaults.Normal.PlayerShipDamageMultiplier.Key, ConfigurationDefaults.Normal.PlayerShipDamageMultiplier.Value);
        Normal.SpawnGroupAmountMultiplier = config.BindValue<float>(ConfigurationDefaults.Normal.SpawnGroupAmountMultiplier.Key, ConfigurationDefaults.Normal.SpawnGroupAmountMultiplier.Value);

        Veteran.EnemyHpMultiplier = config.BindValue<float>(ConfigurationDefaults.Veteran.EnemyHpMultiplier.Key, ConfigurationDefaults.Veteran.EnemyHpMultiplier.Value);
        Veteran.PlayerShipDamageMultiplier = config.BindValue<float>(ConfigurationDefaults.Veteran.PlayerShipDamageMultiplier.Key, ConfigurationDefaults.Veteran.PlayerShipDamageMultiplier.Value);
        Veteran.SpawnGroupAmountMultiplier = config.BindValue<float>(ConfigurationDefaults.Veteran.SpawnGroupAmountMultiplier.Key, ConfigurationDefaults.Veteran.SpawnGroupAmountMultiplier.Value);

        Expert.EnemyHpMultiplier = config.BindValue<float>(ConfigurationDefaults.Expert.EnemyHpMultiplier.Key, ConfigurationDefaults.Expert.EnemyHpMultiplier.Value);
        Expert.PlayerShipDamageMultiplier = config.BindValue<float>(ConfigurationDefaults.Expert.PlayerShipDamageMultiplier.Key, ConfigurationDefaults.Expert.PlayerShipDamageMultiplier.Value);
        Expert.SpawnGroupAmountMultiplier = config.BindValue<float>(ConfigurationDefaults.Expert.SpawnGroupAmountMultiplier.Key, ConfigurationDefaults.Expert.SpawnGroupAmountMultiplier.Value);

        Insane.EnemyHpMultiplier = config.BindValue<float>(ConfigurationDefaults.Insane.EnemyHpMultiplier.Key, ConfigurationDefaults.Insane.EnemyHpMultiplier.Value);
        Insane.PlayerShipDamageMultiplier = config.BindValue<float>(ConfigurationDefaults.Insane.PlayerShipDamageMultiplier.Key, ConfigurationDefaults.Insane.PlayerShipDamageMultiplier.Value);
        Insane.SpawnGroupAmountMultiplier = config.BindValue<float>(ConfigurationDefaults.Insane.SpawnGroupAmountMultiplier.Key, ConfigurationDefaults.Insane.SpawnGroupAmountMultiplier.Value);
    }

    public ulong SettingsVersion { get; set; } = 1705406119ul;

    public DifficultyConfig Normal { get; } = new() { EnemyHpMultiplier = -0.5f, PlayerShipDamageMultiplier = -0.5f };

    public DifficultyConfig Veteran { get; } = new() { EnemyHpMultiplier = -0.25f, PlayerShipDamageMultiplier = -0.25f };

    public DifficultyConfig Expert { get; } = new(); //all values are at default 0

    public DifficultyConfig Insane { get; } = new() { EnemyHpMultiplier = 0.25f, PlayerShipDamageMultiplier = 0.25f };

    internal class DifficultyConfig
    {
        public float EnemyHpMultiplier { get; set; }

        public float PlayerShipDamageMultiplier { get; set; }

        public float SpawnGroupAmountMultiplier { get; set; }

        public override string ToString() => $"Effective Difficulty: {(1 + EnemyHpMultiplier) * (1 + PlayerShipDamageMultiplier) * (1 + SpawnGroupAmountMultiplier) * 100}%";
    }

    static class ConfigurationDefaults
    {
        internal static class Normal
        {
            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> EnemyHpMultiplier =
                new(new(nameof(Normal), nameof(DifficultyConfig.EnemyHpMultiplier)),
                    new($"Difficulty multiplier for {nameof(Normal)} difficulty level {nameof(EnemyHpMultiplier)}. Game default is -0.5f",
                        new AcceptableValueRange<float>(-0.5f, 0f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Normal << (int)Difficulty.Normal + 2,
                            DefaultValue = 0f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> PlayerShipDamageMultiplier =
                new(new(nameof(Normal), nameof(DifficultyConfig.PlayerShipDamageMultiplier)),
                    new($"Difficulty multiplier for {nameof(Normal)} difficulty level {nameof(PlayerShipDamageMultiplier)}. Game default is -0.5f",
                        new AcceptableValueRange<float>(-0.5f, 0f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Normal << (int)Difficulty.Normal + 1,
                            DefaultValue = 0f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> SpawnGroupAmountMultiplier =
                new(new(nameof(Normal), nameof(DifficultyConfig.SpawnGroupAmountMultiplier)),
                    new($"Difficulty multiplier for {nameof(Normal)} difficulty level {nameof(SpawnGroupAmountMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 0.5f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Normal << (int)Difficulty.Normal,
                            DefaultValue = 0f,
                        }));
        }

        internal static class Veteran
        {
            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> EnemyHpMultiplier =
                new(new(nameof(Veteran), nameof(DifficultyConfig.EnemyHpMultiplier)),
                    new($"Difficulty multiplier for {nameof(Veteran)} difficulty level {nameof(EnemyHpMultiplier)}. Game default is -0.25f",
                        new AcceptableValueRange<float>(-0.25f, 0.15f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran + 2,
                            DefaultValue = 0.15f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> PlayerShipDamageMultiplier =
                new(new(nameof(Veteran), nameof(DifficultyConfig.PlayerShipDamageMultiplier)),
                    new($"Difficulty multiplier for {nameof(Veteran)} difficulty level {nameof(PlayerShipDamageMultiplier)}. Game default is -0.25f",
                        new AcceptableValueRange<float>(-0.25f, 0.15f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran + 1,
                            DefaultValue = 0.12f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> SpawnGroupAmountMultiplier =
                new(new(nameof(Veteran), nameof(DifficultyConfig.SpawnGroupAmountMultiplier)),
                    new($"Difficulty multiplier for {nameof(Veteran)} difficulty level {nameof(SpawnGroupAmountMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 0.75f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran,
                            DefaultValue = 0.75f,
                        }));
        }

        internal static class Expert
        {
            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> EnemyHpMultiplier =
                new(new(nameof(Expert), nameof(DifficultyConfig.EnemyHpMultiplier)),
                    new($"Difficulty multiplier for {nameof(Expert)} difficulty level {nameof(EnemyHpMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 0.2f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = 2,
                            DefaultValue = 0.2f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> PlayerShipDamageMultiplier =
                new(new(nameof(Expert), nameof(DifficultyConfig.PlayerShipDamageMultiplier)),
                    new($"Difficulty multiplier for {nameof(Expert)} difficulty level {nameof(PlayerShipDamageMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 0.34f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = 1,
                            DefaultValue = 0.34f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> SpawnGroupAmountMultiplier =
                new(new(nameof(Expert), nameof(DifficultyConfig.SpawnGroupAmountMultiplier)),
                    new($"Difficulty multiplier for {nameof(Expert)} difficulty level {nameof(SpawnGroupAmountMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 1.5f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            DefaultValue = 1.5f,
                        }));
        }

        internal static class Insane
        {
            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> EnemyHpMultiplier =
                new(new(nameof(Insane), nameof(DifficultyConfig.EnemyHpMultiplier)),
                    new($"Difficulty multiplier for {nameof(Insane)} difficulty level {nameof(EnemyHpMultiplier)}. Game default is 0.25f",
                        new AcceptableValueRange<float>(0.25f, 0.3f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = 2,
                            DefaultValue = 0.3f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> PlayerShipDamageMultiplier =
                new(new(nameof(Insane), nameof(DifficultyConfig.PlayerShipDamageMultiplier)),
                    new($"Difficulty multiplier for {nameof(Insane)} difficulty level {nameof(PlayerShipDamageMultiplier)}. Game default is 0.25f",
                        new AcceptableValueRange<float>(0.25f, 0.5f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            Order = 1,
                            DefaultValue = 0.5f,
                        }));

            internal static readonly KeyValuePair<ConfigDefinition, ConfigDescription> SpawnGroupAmountMultiplier =
                new(new(nameof(Insane), nameof(DifficultyConfig.SpawnGroupAmountMultiplier)),
                    new($"Difficulty multiplier for {nameof(Insane)} difficulty level {nameof(SpawnGroupAmountMultiplier)}. Game default is 0f",
                        new AcceptableValueRange<float>(0f, 2.25f),
                        new ConfigurationManagerAttributes
                        {
                            IsAdvanced = true,
                            DefaultValue = 2.25f,
                        }));
        }
    }
}