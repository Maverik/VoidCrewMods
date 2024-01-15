using BepInEx.Bootstrap;
using static HarmonyLib.Harmony;

namespace MavsLibCore;

[SuppressMessage("Class Declaration", "BepInEx001:Class inheriting from BaseUnityPlugin missing BepInPlugin attribute")]
[PublicAPI]
public abstract class MavsBepInExPlugin<T> : BaseUnityPlugin, IMetadata<string> where T : MavsBepInExPlugin<T>, new()
{
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Needed for ref access")]
    protected static byte ExceptionCount;

    string? _harmonyPatcherId;

    protected new static MavLogger<T> Logger => MavLogger<T>.Default;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once StaticMemberInGenericType
    protected static int MaxAllowedExceptionCount { get; set; } = 5;

    public static T? Instance { get; protected set; }

    protected virtual void Awake() =>
        LoggedExceptions(() =>
        {
            Logger.LogDebug("Plugin is awake...");

            if (!string.IsNullOrEmpty(_harmonyPatcherId))
                UnpatchID(_harmonyPatcherId);

            _harmonyPatcherId = CreateAndPatchAll(typeof(T)).Id;

            if (Instance is not null && Instance != this) DestroyImmediate(Instance);

            Instance = (T?)this;

            Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;

            Logger.LogInfo("Plugin initialized.");
        });

    protected virtual void OnDestroy()
    {
        Logger.LogInfo("Plugin is unloading...");

        LoggedExceptions(() =>
        {
            Instance = null;

            if (!string.IsNullOrEmpty(_harmonyPatcherId))
                UnpatchID(_harmonyPatcherId);

            Logger.LogInfo("Plugin has unloaded.");
        });
    }

    public abstract string DisplayName { get; }

    public abstract Version Version { get; }

    public abstract string Id { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void LoggedExceptions(Action? action)
    {
        try
        {
            if (ExceptionCount < MaxAllowedExceptionCount && action is not null) action();
        }
        catch (Exception ex)
        {
            ExceptionCount++;

            var iex = ex;

            while (iex.InnerException is not null) iex = iex.InnerException;

            Logger.LogError($"Exception count tripped {ExceptionCount} of {MaxAllowedExceptionCount}: {iex.Message}\r\n{ex.Message}\r\n-- Innermost stacktrace:\r\n{iex.StackTrace}\r\n-- Stacktrace:\r\n{ex.StackTrace}");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue? LoggedExceptions<TValue>(Func<TValue>? action, TValue? exceptionValue = default)
    {
        try
        {
            return ExceptionCount < MaxAllowedExceptionCount && action is not null ? action() : exceptionValue;
        }
        catch (Exception ex)
        {
            ExceptionCount++;

            var iex = ex;

            while (iex.InnerException is not null) iex = iex.InnerException;

            Logger.LogError($"Exception count tripped {ExceptionCount} of {MaxAllowedExceptionCount}: {iex.Message}\r\n{ex.Message}\r\n-- Innermost stacktrace:\r\n{iex.StackTrace}\r\n-- Stacktrace:\r\n{ex.StackTrace}");

            return exceptionValue;
        }
    }

    public TConfig? RefreshConfiguration<TConfig, TKey>(TConfig referenceConfig) where TConfig : IVersionedMetadata<TKey> where TKey : IEquatable<TKey> =>
        LoggedExceptions(() =>
        {
            var store = KVStorage<TConfig, TKey>.Default;

            var config = store.Load(referenceConfig.Id);

            if (config is null || config.SchemaRevision != referenceConfig.SchemaRevision || !config.Version.Equals(referenceConfig.Version))
            {
                Logger.LogDebug("Metadata has changed. Forcing configuration update.");

                var success = store.Store(referenceConfig);

                if (success) config = referenceConfig;

                Logger.LogDebug($"Config update: {success}");
            }

            Logger.LogMessage($"Configuration successfully loaded: {config}");

            return config;
        });
}

#pragma warning disable BepInEx001
public abstract class MavsBepInExPlugin<T, TConfig> : MavsBepInExPlugin<T> where T : MavsBepInExPlugin<T>, new() where TConfig : ISupportsConfigFile, new()
#pragma warning restore BepInEx001
{
    protected static TConfig Configuration { get; set; } = new();

    public virtual void Start() => LoggedExceptions(() =>
    {
        Logger.LogDebug("Configuration loading...");

        Logger.LogDebug($"Configuration was: {JsonConvert.SerializeObject(Configuration, MavsDefaults.DefaultJsonSerializerOptions)}");

        Configuration.LoadFrom(Config);

        Logger.LogDebug($"Configuration is: {JsonConvert.SerializeObject(Configuration, MavsDefaults.DefaultJsonSerializerOptions)}");

        Logger.LogInfo("Configuration loaded.");
    });
}