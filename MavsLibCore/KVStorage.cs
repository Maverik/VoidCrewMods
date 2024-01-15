namespace MavsLibCore;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class KVStorage<T, TKey> where T : IKey<TKey> where TKey : IEquatable<TKey>
{
    public static MavLogger<T> Logger => MavLogger<T>.Default;

    public static KVStorage<T, TKey> Default { get; } = new();

    public static int ExceptionCount { get; set; }

    public static int MaxAllowedExceptionCount => 5;

    public T? Load(TKey keyId) =>
        LoggedExceptions(() =>
        {
            var path = Path.Combine(Paths.ConfigPath, $"{keyId}.json");
            using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            var jsonContent = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(jsonContent, MavsDefaults.DefaultJsonSerializerOptions);
        });

    public bool Store(T entry) =>
        LoggedExceptions(() =>
        {
            var path = Path.Combine(Paths.ConfigPath, $"{entry.Id}.json");
            using var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream, Encoding.UTF8);

            var jsonContent = JsonConvert.SerializeObject(entry, MavsDefaults.DefaultJsonSerializerOptions);

            writer.Write(jsonContent);
            writer.Flush();

            return true;
        });

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
}