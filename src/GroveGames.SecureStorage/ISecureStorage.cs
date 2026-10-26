using System.Diagnostics.CodeAnalysis;

namespace GroveGames.SecureStorage;

public interface ISecureStorage
{
    public bool TryRead(string key, [NotNullWhen(true)] out string? value);
    public bool Write(string key, string value);
    public bool Delete(string key);
}
