using UnityEngine;

namespace BetterScoop;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterScoop : MavsBepInExPlugin<BetterScoop>
{
    static readonly Type _carryableAttractorType = typeof(CarryableAttractor);
    static readonly FieldInfo _itemSearchInterval = AccessTools.Field(_carryableAttractorType, "_itemSearchInterval");
    static readonly FieldInfo _itemSearchInitialDelay = AccessTools.Field(_carryableAttractorType, "_itemSearchInitialDelay");
    static readonly FieldInfo _coneAngle = AccessTools.Field(_carryableAttractorType, "_coneAngle");
    static readonly FieldInfo _maxRange = AccessTools.Field(_carryableAttractorType, "_maxRange");
    static readonly FieldInfo _pullVelocity = AccessTools.Field(_carryableAttractorType, "_pullVelocity");
    static readonly FieldInfo _catchRadius = AccessTools.Field(_carryableAttractorType, "_catchRadius");
    static readonly FieldInfo _inheritVelocityValue = AccessTools.Field(_carryableAttractorType, "_inheritVelocityValue");

    public static PluginConfiguration Configuration { get; set; }

    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    void Start() => Configuration = new(Config);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CarryableAttractor), "Awake")]
    static void CarryableAttractorAwakePrefix(CarryableAttractor __instance)
    {
        if (PhotonNetwork.IsMasterClient)
            LoggedExceptions(() =>
            {
                __instance.Set<CarryableAttractor, float>(_itemSearchInterval, _ => Configuration.ItemSearchInterval.Value, Logger);
                __instance.Set<CarryableAttractor, float>(_itemSearchInitialDelay, _ => Configuration.ItemSearchInitialDelay.Value, Logger);
                __instance.Set<CarryableAttractor, float>(_coneAngle, _ => Configuration.ConeAngle.Value, Logger);
                __instance.Set<CarryableAttractor, float>(_catchRadius, _ => Configuration.CatchRadius.Value, Logger);
                __instance.Set<CarryableAttractor, float>(_inheritVelocityValue, _ => Configuration.InheritVelocityValue.Value, Logger);
                __instance.Set<CarryableAttractor, ModifiableFloat>(_maxRange, x => x.Set<ModifiableFloat, float, FloatModifier>(Configuration.MaxRange.Value, Logger)!, Logger);
                __instance.Set<CarryableAttractor, ModifiableFloat>(_pullVelocity, x => x.Set<ModifiableFloat, float, FloatModifier>(Configuration.PullVelocity.Value, Logger)!, Logger);

                Logger.LogMessage("Scoop successfully supercharged!");
            });
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GravityScoopModule), "OnAcquireCarryable")]
    static void GravityScoopModuleOnAcquireCarryablePrefix(GravityScoopModule __instance, AbstractCarryableObject carryable)
    {
        Logger.LogDebug($"GravityScoopModule.OnAcquireCarryable: {carryable}");
        Logger.LogDebug($"GravityScoopModule.OnAcquireCarryable: {carryable.DisplayName}, att: {carryable.IsBeingAttracted}, persist: {carryable.PersistantObject}, Id: {carryable.Identifier}");
        Logger.LogDebug($"GravityScoopModule.OnAcquireCarryable: {carryable.ExtraJData}");
    }
}