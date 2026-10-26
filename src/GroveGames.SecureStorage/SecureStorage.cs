using System.Diagnostics.CodeAnalysis;

namespace GroveGames.SecureStorage;

public sealed class SecureStorage : ISecureStorage
{
    private readonly ISecureStore _store;

    public SecureStorage(ISecureStore store)
    {
        ArgumentNullException.ThrowIfNull(store);
        _store = store;
    }

    public bool TryRead(string key, [NotNullWhen(true)] out string? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        return _store.TryRead(key, out value);
    }

    public bool Write(string key, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        ArgumentNullException.ThrowIfNull(value);
        return _store.Write(key, value);
    }

    public bool Delete(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        return _store.Delete(key);
    }
}
