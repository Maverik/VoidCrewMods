using CG.Client.Quests;
using CG.Game.SpaceObjects.Controllers;
using Gameplay.MissionDifficulty;

namespace HigherDifficulty;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class HigherDifficulty : MavsBepInExPlugin<HigherDifficulty, PluginConfiguration>
{
    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SwarmController), "Start")]
    static void SwarmControllerStartPrefix(SwarmController __instance)
    {
        if (PhotonNetwork.IsMasterClient)
            LoggedExceptions(() =>
            {
                var droneCount = Convert.ToInt32(Math.Clamp(GameSessionManager.ActiveSession.Difficulty switch
                {
                    Difficulty.Normal => 1 + Configuration.Normal.SpawnGroupAmountMultiplier,
                    Difficulty.Veteran => 1 + Configuration.Veteran.SpawnGroupAmountMultiplier,
                    Difficulty.Expert => 1 + Configuration.Expert.SpawnGroupAmountMultiplier,
                    Difficulty.Insane => 1 + Configuration.Insane.SpawnGroupAmountMultiplier,
                    _ => 1
                } * __instance.InitialDroneCount * 0.625f, __instance.InitialDroneCount, __instance.InitialDroneCount * 2.25f));

                SetExtensions.Set(ref __instance.InitialDroneCount, droneCount, Logger);
            });
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(HubQuestManager), "Awake")]
    static void HubQuestManagerAwakePrefix()
    {
        if (PhotonNetwork.IsMasterClient)
            LoggedExceptions(() =>
            {
                SetConfig(Difficulty.Normal, Configuration.Normal);
                SetConfig(Difficulty.Veteran, Configuration.Veteran);
                SetConfig(Difficulty.Expert, Configuration.Expert);
                SetConfig(Difficulty.Insane, Configuration.Insane);

                Logger.LogMessage("Difficulties supercharged!");
            });
    }

    static void SetConfig(Difficulty difficulty, PluginConfiguration.DifficultyConfig config)
    {
        var rac = ResourceAssetContainer<QuestDifficultyContainer, QuestDifficulty, QuestDifficultyDef>.Instance.GetConfig(difficulty);
        var assetConfig = rac.Asset.Config;

        Logger.LogDebug($"DifficultyConfig for {rac.ContextInfo.HeaderText} was: {assetConfig}");

        assetConfig.EnemyHpMultiplier.Set(config.EnemyHpMultiplier, Logger, $"{difficulty} EnemyHpMultiplier");
        assetConfig.PlayerShipDamageMultiplier.Set(config.PlayerShipDamageMultiplier, Logger, $"{difficulty} PlayerShipDamageMultiplier");
        assetConfig.SpawnGroupAmountMultiplier.Set(config.SpawnGroupAmountMultiplier, Logger, $"{difficulty} SpawnGroupAmountMultiplier");
        
        Logger.LogDebug($"Config effective requested increase: {config}");

        rac = ResourceAssetContainer<QuestDifficultyContainer, QuestDifficulty, QuestDifficultyDef>.Instance.GetConfig(difficulty);

        Logger.LogInfo($"DifficultyConfig for {rac.ContextInfo.HeaderText} is: {rac.Asset.Config}");
    }
}