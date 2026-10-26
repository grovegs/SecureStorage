namespace GroveGames.SecureStorage.Unity
{
    public sealed class SecureStorage : ISecureStorage
    {
        private readonly global::GroveGames.SecureStorage.SecureStorage _storage;

        public SecureStorage()
        {
            _storage = new global::GroveGames.SecureStorage.SecureStorage(CreateStore());
        }

        public bool TryRead(string key, out string value)
        {
            return _storage.TryRead(key, out value);
        }

        public bool Write(string key, string value)
        {
            return _storage.Write(key, value);
        }

        public bool Delete(string key)
        {
            return _storage.Delete(key);
        }

        private static ISecureStore CreateStore()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return new IosSecureStore();
#elif UNITY_ANDROID && !UNITY_EDITOR
            return new AndroidSecureStore();
#else
            return new PlayerPrefsSecureStore();
#endif
        }
    }
}
