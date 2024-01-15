using CG.Ship.Shield;

namespace BetterShields;

[BepInPlugin(PluginInfo.PackageId, PluginInfo.Title, PluginInfo.VersionString)]
[DisallowMultipleComponent]
sealed class BetterShields : MavsBepInExPlugin<BetterShields, PluginConfiguration>
{
    static readonly ConstructorInfo _ctor = typeof(ShieldConfig).GetConstructor([
        typeof(float), typeof(float), typeof(float),
        typeof(float)
    ]) ?? throw new InvalidOperationException("ShieldConfig Constructor Not Found");

    static readonly CodeMatch[] _codeMatches =
    [
        //IL_0000: ldc.r4 10
        new(OpCodes.Ldc_R4, 10f),
        // IL_0005: ldc.r4 1
        new(OpCodes.Ldc_R4, 1f),
        // IL_000a: ldc.r4 5
        new(OpCodes.Ldc_R4, 5f),
        // IL_000f: ldc.r4 0.5
        new(OpCodes.Ldc_R4, 0.5f),
        // IL_0014: newobj instance void CG.Ship.Shield.ShieldConfig::.ctor(float32, float32, float32, float32)
        new(OpCodes.Newobj, _ctor)
    ];

    public override string DisplayName => PluginInfo.Title;

    public override Version Version => PluginInfo.Version;

    public override string Id => PluginInfo.PackageId;

    [HarmonyPatch(typeof(ShieldConfig), nameof(ShieldConfig.Default), MethodType.Getter)]
    [HarmonyTranspiler]
    static List<CodeInstruction> ShieldConfigTranspile(IEnumerable<CodeInstruction> instructions)
        => LoggedExceptions(() => new CodeMatcher(instructions).MatchForward(false, _codeMatches)
            .ThrowIfInvalid("Code sequence could not be found for ShieldConfig.Default patching")
            .SetOperandAndAdvance(Configuration.HitPoints)
            .SetOperandAndAdvance(Configuration.RechargeSpeed)
            .SetOperandAndAdvance(Configuration.RechargeDelay)
            .SetOperandAndAdvance(Configuration.AbsorptionRate)
            .Instructions())!;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ShieldDamagable), nameof(ShieldDamagable.Start))]
    static void ShieldDamagableStartPrefix(ShieldDamagable __instance)
    {
        if (PhotonNetwork.IsMasterClient)
            LoggedExceptions(() =>
            {
                Logger.LogDebug($"ShieldDamagable was {__instance}");

                __instance.Set(x => x.Absorption, _ => Configuration.AbsorptionRate, Logger);
                __instance.Set(x => x.maxAbsorption, _ => Configuration.AbsorptionRate, Logger);
                __instance.Set(x => x.HitPoints, _ => Configuration.HitPoints, Logger);
                __instance.Set(x => x.MaxHitPoints, _ => Configuration.HitPoints, Logger);
                __instance.Set(x => x.RechargeDelay, _ => Configuration.RechargeDelay, Logger);
                __instance.Set(x => x.RechargeSpeed, _ => Configuration.RechargeSpeed, Logger);

                Logger.LogInfo($"ShieldDamagable is {__instance}");

                Logger.LogMessage("Shields supercharged!");
            });
    }
}