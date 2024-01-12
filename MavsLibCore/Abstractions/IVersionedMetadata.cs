namespace MavsLibCore;

public interface IVersionedMetadata<out TKey> : IMetadata<TKey> where TKey : IEquatable<TKey>
{
    uint SchemaRevision { get; }
}