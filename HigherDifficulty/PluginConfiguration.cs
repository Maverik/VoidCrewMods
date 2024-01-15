using Gameplay.MissionDifficulty;

namespace HigherDifficulty;

sealed class PluginConfiguration : ISupportsConfigFile
{
    public void LoadFrom(ConfigFile config)
    {
        var difficultyConfig = Normal;

        difficultyConfig.EnemyHpMultiplier = config.Bind(nameof(Normal), nameof(DifficultyConfig.EnemyHpMultiplier), 0f,
                new ConfigDescription("Difficulty multiplier for Normal difficulty level EnemyHpMultiplier. Game default is -0.5f", new AcceptableValueRange<float>(-0.5f, 0f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Normal << (int)Difficulty.Normal + 2 }))
            .Value;

        difficultyConfig.PlayerShipDamageMultiplier = config.Bind(nameof(Normal), nameof(DifficultyConfig.PlayerShipDamageMultiplier), 0f,
                new ConfigDescription("Difficulty multiplier for Normal difficulty level PlayerShipDamageMultiplier. Game default is -0.5f", new AcceptableValueRange<float>(-0.5f, 0f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Normal << (int)Difficulty.Normal + 1 }))
            .Value;
        difficultyConfig.SpawnGroupAmountMultiplier = config.Bind(nameof(Normal), nameof(DifficultyConfig.SpawnGroupAmountMultiplier), 0f,
                new ConfigDescription("Difficulty multiplier for Normal difficulty level SpawnGroupAmountMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 2.25f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Normal << (int)Difficulty.Normal }))
            .Value;

        difficultyConfig = Veteran;

        difficultyConfig.EnemyHpMultiplier = config.Bind(nameof(Veteran), nameof(DifficultyConfig.EnemyHpMultiplier), 0.15f,
                new ConfigDescription("Difficulty multiplier for Veteran difficulty level EnemyHpMultiplier. Game default is -0.25f", new AcceptableValueRange<float>(-0.25f, 0.15f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran + 2 }))
            .Value;
        difficultyConfig.PlayerShipDamageMultiplier = config.Bind(nameof(Veteran), nameof(DifficultyConfig.PlayerShipDamageMultiplier), 0.15f,
                new ConfigDescription("Difficulty multiplier for Veteran difficulty level PlayerShipDamageMultiplier. Game default is -0.25f", new AcceptableValueRange<float>(-0.25f, 0.15f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran + 1 }))
            .Value;
        difficultyConfig.SpawnGroupAmountMultiplier = config.Bind(nameof(Veteran), nameof(DifficultyConfig.SpawnGroupAmountMultiplier), 1f,
                new ConfigDescription("Difficulty multiplier for Veteran difficulty level SpawnGroupAmountMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 2.25f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Veteran << (int)Difficulty.Veteran }))
            .Value;

        difficultyConfig = Expert;

        difficultyConfig.EnemyHpMultiplier = config.Bind(nameof(Expert), nameof(DifficultyConfig.EnemyHpMultiplier), 0.25f,
                new ConfigDescription("Difficulty multiplier for Expert difficulty level EnemyHpMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 0.25f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Expert << (int)Difficulty.Expert + 2 }))
            .Value;
        difficultyConfig.PlayerShipDamageMultiplier = config.Bind(nameof(Expert), nameof(DifficultyConfig.PlayerShipDamageMultiplier), 0.325f,
                new ConfigDescription("Difficulty multiplier for Expert difficulty level PlayerShipDamageMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 0.325f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Expert << (int)Difficulty.Expert + 1 }))
            .Value;
        difficultyConfig.SpawnGroupAmountMultiplier = config.Bind(nameof(Expert), nameof(DifficultyConfig.SpawnGroupAmountMultiplier), 2.25f,
                new ConfigDescription("Difficulty multiplier for Expert difficulty level SpawnGroupAmountMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 2.25f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Expert << (int)Difficulty.Expert }))
            .Value;

        difficultyConfig = Insane;

        difficultyConfig.EnemyHpMultiplier = config.Bind(nameof(Insane), nameof(DifficultyConfig.EnemyHpMultiplier), 0.25f,
                new ConfigDescription("Difficulty multiplier for Insane difficulty level EnemyHpMultiplier. Game default is 0.25f", new AcceptableValueRange<float>(0.25f, 0.35f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Insane << (int)Difficulty.Insane + 2 }))
            .Value;
        difficultyConfig.PlayerShipDamageMultiplier = config.Bind(nameof(Insane), nameof(DifficultyConfig.PlayerShipDamageMultiplier), 0.5f,
                new ConfigDescription("Difficulty multiplier for Insane difficulty level PlayerShipDamageMultiplier. Game default is 0.25f", new AcceptableValueRange<float>(0.25f, 0.65f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Insane << (int)Difficulty.Insane + 1 }))
            .Value;
        difficultyConfig.SpawnGroupAmountMultiplier = config.Bind(nameof(Insane), nameof(DifficultyConfig.SpawnGroupAmountMultiplier), 2.25f,
                new ConfigDescription("Difficulty multiplier for Insane difficulty level SpawnGroupAmountMultiplier. Game default is 0f", new AcceptableValueRange<float>(0f, 2.25f), new ConfigurationManagerAttributes { IsAdvanced = true, Order = (int)Difficulty.Insane << (int)Difficulty.Insane }))
            .Value;
    }

    public DifficultyConfig Normal { get; } = new() { EnemyHpMultiplier = -0.5f, PlayerShipDamageMultiplier = -0.5f };

    public DifficultyConfig Veteran { get; } = new() { EnemyHpMultiplier = -0.25f, PlayerShipDamageMultiplier = -0.25f };

    public DifficultyConfig Expert { get; } = new(); //all values are at default 0

    public DifficultyConfig Insane { get; } = new() { EnemyHpMultiplier = 0.25f, PlayerShipDamageMultiplier = 0.25f };

    internal class DifficultyConfig
    {
        public float EnemyHpMultiplier { get; set; }

        public float PlayerShipDamageMultiplier { get; set; }

        public float SpawnGroupAmountMultiplier { get; set; }
    }
}