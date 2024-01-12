using BepInEx.Logging;
using Logger = BepInEx.Logging.Logger;

namespace MavsLibCore;

public class MavLogger<T>() : MavLogger(typeof(T).Name)
{
    static MavLogger()
    {
        Default = new();
        Logger.Sources.Add(Default);
    }

    public new static MavLogger<T> Default { get; }
}

public class MavLogger(string sourceName) : ManualLogSource(sourceName)
{
    static MavLogger()
    {
        Default = new(SharedLoggerName);
        Logger.Sources.Add(Default);
    }

    public static string SharedLoggerName => "Mavs Common Logger";

    public static MavLogger Default { get; }
}