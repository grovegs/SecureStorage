using System.Diagnostics.CodeAnalysis;

namespace GroveGames.SecureStorage;

public sealed class InMemorySecureStore : ISecureStore
{
    private readonly Dictionary<string, string> _values;

    public InMemorySecureStore()
    {
        _values = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    public bool TryRead(string key, [NotNullWhen(true)] out string? value)
    {
        return _values.TryGetValue(key, out value);
    }

    public bool Write(string key, string value)
    {
        _values[key] = value;
        return true;
    }

    public bool Delete(string key)
    {
        _values.Remove(key);
        return true;
    }
}
