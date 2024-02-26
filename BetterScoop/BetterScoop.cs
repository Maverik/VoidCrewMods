namespace BetterScoop;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterScoop : MavsBepInExPlugin<BetterScoop, PluginConfiguration>
{
    static readonly Type _carryableAttractorType = typeof(CarryableAttractor);
    static readonly FieldInfo _coneAngle = AccessTools.Field(_carryableAttractorType, "_coneAngle");
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

    static readonly MethodInfo _getGameObject = AccessTools.PropertyGetter(typeof(UnityEngine.Component), "gameObject") ?? throw new ArgumentNullException("get_gameObject", "gameObject property getter was not found");
    static readonly MethodInfo _updateIntervaled = AccessTools.Method(_carryableAttractorType, "UpdateIntervaled") ?? throw new ArgumentNullException("UpdateIntervaled", "UpdateIntervaled method was not found");

    static readonly ConstructorInfo _actionContructor = AccessTools.Constructor(typeof(Action), [typeof(object), typeof(IntPtr)]);

    static readonly CodeMatch[] _itemSearchIntervalCodeMatches =
    [
        //IL_0026: ldarg.0
        new(OpCodes.Ldarg_0),

        //IL_0027: ldc.r4 0.25
        new(OpCodes.Ldc_R4, 0.25f),

        //IL_002c: ldarg.0
        new(OpCodes.Ldarg_0),

        //IL_002d: call instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
        new(OpCodes.Call, _getGameObject),

        // IL_0032: ldarg.0
        new(OpCodes.Ldarg_0),

        //IL_0033: ldftn instance void CG.Ship.Modules.CarryableAttractor::UpdateIntervaled()
        new(OpCodes.Ldftn, _updateIntervaled),

        //IL_0039: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
        new(OpCodes.Newobj, _actionContructor),

        //IL_003e: ldc.r4 0.5
        new(OpCodes.Ldc_R4, 0.5f)
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
                scoop.Set(x => x.MaxRange, _ => Configuration.MaxRange, Logger, nameof(Configuration.MaxRange));
                scoop.Set<CarryableAttractor, float>(_coneAngle, _ => Configuration.ConeAngle, Logger, nameof(Configuration.ConeAngle));
                scoop.Set<CarryableAttractor, float>(_catchRadius, _ => Configuration.CatchRadius, Logger, nameof(Configuration.CatchRadius));
                scoop.Set<CarryableAttractor, float>(_inheritVelocityValue, _ => Configuration.InheritVelocityValue, Logger, nameof(Configuration.InheritVelocityValue));
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

#if DEBUG
                Logger.LogDebug(JsonConvert.SerializeObject(__result.Select(x => new
                {
                    x.DisplayName,
                    x.ContextInfo.Header,
                    x.assetGuid,
                    x.UseCollision,
                    ColliderSizes = string.Join(',', x.Colliders.OfType<BoxCollider>().Select(xx => xx.size)),
                    x.ContainerGuid
                })));
#endif
                return __result;
            }))!;

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(CarryableAttractor), "SubscribeAsMaster")]
    static IEnumerable<CodeInstruction> CarryableAttractorSubscribeAsMasterTranspiler(IEnumerable<CodeInstruction> instructions) =>
        LoggedExceptions(() =>
        {

            var config = new PluginConfiguration();
            // ReSharper disable once Unity.IncorrectMonoBehaviourInstantiation
            // Not meant to instantiated as behaviour but only to pull the config out of bepinex
            config.LoadFrom(new BetterScoop().Config);
            
            var patchedInstructions = new CodeMatcher(instructions).MatchForward(false, _itemSearchIntervalCodeMatches)
                    .ThrowIfInvalid("Code sequence could not be found for CarryableAttractor.SubscribeAsMaster patching")
                    .Advance(1)
                    .SetOperandAndAdvance(config.ItemSearchInterval) //frequency
                    .Advance(5)
                    .SetOperandAndAdvance(config.ItemSearchInitialDelay) //delay
                    .InstructionEnumeration()
#if DEBUG
                    .Select(x =>
                    {
                        Logger.LogDebug(x.ToString());

                        return x;
                    })
#endif
                ;

            Logger.LogInfo($"CarryableAttractor.ItemSearchInterval patched from 0.25f to {config.ItemSearchInterval}f");
            Logger.LogInfo($"CarryableAttractor.ItemSearchInitialDelay patched from 0.5f to {config.ItemSearchInitialDelay}f");

            return patchedInstructions;
        })!;
}