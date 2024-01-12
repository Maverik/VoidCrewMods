namespace MavsLibCore;

public interface IMetadata<out TKey> : IKey<TKey> where TKey : IEquatable<TKey>
{
    string DisplayName { get; }

    Version Version { get; }
}