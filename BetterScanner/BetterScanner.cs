using CG.Ship.Scanning;

namespace BetterScanner;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterScanner : MavsBepInExPlugin<BetterScanner, PluginConfiguration>
{
    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ScanComponent), "Awake")]
    static void ScanComponentAwakePrefix(ScanComponent __instance) => LoggedExceptions(() =>
    {
        __instance.Set(x => x.HelmScanningRange, _ => Configuration.HelmScanningRange, Logger, "ScanComponent");

        if (PhotonNetwork.IsMasterClient)
            ClientGame.Current.PlayerShip.Set(x => x.passiveScanRadius, _ => Configuration.PassiveScanningRange, Logger);

        Logger.LogMessage("Helm Scanning Range successfully supercharged!");
    });
}