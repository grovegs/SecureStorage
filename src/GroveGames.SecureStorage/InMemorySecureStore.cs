namespace GroveGames.SecureStorage;

public sealed class InMemorySecureStore : ISecureStore
{
    private readonly Dictionary<string, string> _values;

    public InMemorySecureStore()
    {
        _values = new Dictionary<string, string>(StringComparer.Ordinal);
    }

    public SecureStorageReadResult Read(string key, out string? value)
    {
        return _values.TryGetValue(key, out value) ? SecureStorageReadResult.Found : SecureStorageReadResult.Missing;
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
