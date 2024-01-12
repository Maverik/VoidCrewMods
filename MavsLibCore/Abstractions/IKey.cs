namespace MavsLibCore;

public interface IKey<out TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; }
}