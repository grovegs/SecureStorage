namespace GroveGames.SecureStorage.Unity
{
    public interface ISecureStorage
    {
        bool TryRead(string key, out string value);
        bool Write(string key, string value);
        bool Delete(string key);
    }
}
