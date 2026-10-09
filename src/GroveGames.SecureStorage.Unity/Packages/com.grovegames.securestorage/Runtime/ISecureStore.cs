namespace GroveGames.SecureStorage.Unity
{
    internal interface ISecureStore
    {
        bool TryRead(string key, out string value);
        bool Write(string key, string value);
        bool Delete(string key);
    }
}
