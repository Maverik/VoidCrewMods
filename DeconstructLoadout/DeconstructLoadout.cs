using CG.Client.Player.Interactions.Build;
using CG.Ship.Modules.Shield;
using CG.Ship.Modules.Weapons;
using CG.Space;
using Gameplay.CompositeWeapons;

namespace DeconstructLoadout;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class DeconstructLoadout : MavsBepInExPlugin<DeconstructLoadout>
{
    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ShipLoadout), nameof(ShipLoadout.InitializeShip))]
    static void ShipLoadoutInitializeShipPostfix(GameObject __result)
    {
        if (PhotonNetwork.IsMasterClient && GameSessionManager.HasActiveSession)
            LoggedExceptions(() =>
            {
                var abstractPlayerControlledShip = __result.GetComponent<AbstractPlayerControlledShip>();
                foreach (var cell in abstractPlayerControlledShip.GetModules<CellModule>())
                    switch (cell)
                    {
                        case WeaponModule: //Kinetic defense, rocket launcher
                        case CompositeWeaponModule: //benediction cannon etc..
                        case GravityScoopModule:
                        case PowerGeneratorModule:
                        case ChargeStationModule:
                        case ShieldModule:
                            cell.BuildingConstraints.allowDeconstruction = true;
                            BuildProcessController.Instance.DeconstructModule(cell);
                            Logger.LogMessage($"{cell.ContextInfo.Header} is being deconstructed: {cell.IsBeingDeconstructed}");

                            break;
                    }
            });
    }
}