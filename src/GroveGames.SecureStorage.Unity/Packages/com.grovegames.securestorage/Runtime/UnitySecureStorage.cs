namespace GroveGames.SecureStorage.Unity
{
    public sealed class UnitySecureStorage : ISecureStorage
    {
        private readonly SecureStorage _storage;

        public UnitySecureStorage()
        {
            _storage = new SecureStorage(CreateStore());
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
