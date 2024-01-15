using CG.Ship.Scanning;

namespace BetterScanner;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterScanner : MavsBepInExPlugin<BetterScanner, PluginConfiguration>
{
    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScanComponent), "Awake")]
    static void ScanComponentCtorPostfix(ScanComponent __instance) => LoggedExceptions(() =>
    {
        if (PhotonNetwork.IsMasterClient)
            ClientGame.Current.PlayerShip.Set(x => x.passiveScanRadius, _ => Configuration.PassiveScanningRange, Logger);

        __instance.Set(x => x.HelmScanningRange, _ => Configuration.HelmScanningRange, Logger);
        
        Logger.LogMessage("Helm Scanning Range successfully supercharged!");
    });
}