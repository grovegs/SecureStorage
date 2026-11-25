namespace GroveGames.SecureStorage;

public interface ISecureStorage
{
    public SecureStorageReadResult Read(string key, out string? value);
    public bool Write(string key, string value);
    public bool Delete(string key);
}
