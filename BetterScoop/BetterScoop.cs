namespace BetterScoop;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterScoop : MavsBepInExPlugin<BetterScoop, PluginConfiguration>
{
    static readonly Type _carryableAttractorType = typeof(CarryableAttractor);
    static readonly FieldInfo _itemSearchInterval = AccessTools.Field(_carryableAttractorType, "_itemSearchInterval");
    static readonly FieldInfo _itemSearchInitialDelay = AccessTools.Field(_carryableAttractorType, "_itemSearchInitialDelay");
    static readonly FieldInfo _coneAngle = AccessTools.Field(_carryableAttractorType, "_coneAngle");
    static readonly FieldInfo _maxRange = AccessTools.Field(_carryableAttractorType, "_maxRange");
    static readonly FieldInfo _pullVelocity = AccessTools.Field(_carryableAttractorType, "_pullVelocity");
    static readonly FieldInfo _catchRadius = AccessTools.Field(_carryableAttractorType, "_catchRadius");
    static readonly FieldInfo _inheritVelocityValue = AccessTools.Field(_carryableAttractorType, "_inheritVelocityValue");

    static readonly HashSet<GUIDUnion> _assetGuidBlacklist =
    [
        // Power Fuse - ec0fc0790a706ef4facab39da5d9de04 - Prefabs/Space Objects/Carryables/Item_PowerFuse
        new("ec0fc0790a706ef4facab39da5d9de04"),
        // Oxygen Tank - 6c37b5363f7ef7844881a301dca76572 - Prefabs/Space Objects/Carryables/Item_OxygenTank
        new("6c37b5363f7ef7844881a301dca76572"),
        // Lure - 8124ed58f064e384cb0314005b84be1b - Prefabs/Space Objects/Carryables/Item_EnemyLurer_Small
        new("8124ed58f064e384cb0314005b84be1b"),
        // Lure - ee69440bbce371e458daeba6eee12a49 - Prefabs/Space Objects/Carryables/Item_EnemyLurer
        new("ee69440bbce371e458daeba6eee12a49")
    ];

    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CarryableAttractor), "Awake")]
    static void CarryableAttractorAwakePrefix(CarryableAttractor __instance)
    {
        var scoop = __instance;

        if (PhotonNetwork.IsMasterClient)
            LoggedExceptions(() =>
            {
                scoop.Set<CarryableAttractor, float>(_itemSearchInterval, _ => Configuration.ItemSearchInterval, Logger, nameof(Configuration.ItemSearchInterval));
                scoop.Set<CarryableAttractor, float>(_itemSearchInitialDelay, _ => Configuration.ItemSearchInitialDelay, Logger, nameof(Configuration.ItemSearchInitialDelay));
                scoop.Set<CarryableAttractor, float>(_coneAngle, _ => Configuration.ConeAngle, Logger, nameof(Configuration.ConeAngle));
                scoop.Set<CarryableAttractor, float>(_catchRadius, _ => Configuration.CatchRadius, Logger, nameof(Configuration.CatchRadius));
                scoop.Set<CarryableAttractor, float>(_inheritVelocityValue, _ => Configuration.InheritVelocityValue, Logger, nameof(Configuration.InheritVelocityValue));
                scoop.Set<CarryableAttractor, ModifiableFloat>(_maxRange, _ => Configuration.MaxRange, Logger, nameof(Configuration.MaxRange));
                scoop.Set<CarryableAttractor, ModifiableFloat>(_pullVelocity, _ => Configuration.PullVelocity, Logger, nameof(Configuration.PullVelocity));

                Logger.LogMessage("Scoop successfully supercharged!");
            });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarryableAttractor), "GetPossibleItemsToAttrack")]
    static List<AbstractCarryableObject> CarryableAttractorGetPossibleItemsToAttrackPostfix(List<AbstractCarryableObject> __result) =>
        (__result.Count == 0
            ? __result
            : LoggedExceptions(() =>
            {
                if (!Configuration.PullMissionItems) __result.RemoveAll(x => _assetGuidBlacklist.Contains(x.assetGuid));

                foreach (var item in __result)
                    item.UseCollision = true;

            Logger.LogDebug(JsonConvert.SerializeObject(carryables.Select(x => new
            {
                x.DisplayName,
                x.ContextInfo.HeaderText,
                x.assetGuid,
                x.UseCollision,
                x.ContainerGuid
            })));
        });
    }
#if DEBUG
                Logger.LogDebug(JsonConvert.SerializeObject(__result.Select(x => new
                {
                    x.DisplayName,
                    x.ContextInfo.HeaderText,
                    x.assetGuid,
                    x.UseCollision,
                    ColliderSizes = string.Join(',', x.Colliders.OfType<BoxCollider>().Select(xx => xx.size)),
                    x.ContainerGuid
                })));
#endif
                return __result;
            }))!;
}