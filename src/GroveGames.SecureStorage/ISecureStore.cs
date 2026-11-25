namespace GroveGames.SecureStorage;

public interface ISecureStore
{
    public SecureStorageReadResult Read(string key, out string? value);
    public bool Write(string key, string value);
    public bool Delete(string key);
}
