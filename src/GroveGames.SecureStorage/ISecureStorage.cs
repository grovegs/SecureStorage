using System.Diagnostics.CodeAnalysis;

namespace GroveGames.SecureStorage;

public interface ISecureStorage
{
    bool TryRead(string key, [NotNullWhen(true)] out string? value);
    bool Write(string key, string value);
    bool Delete(string key);
}
