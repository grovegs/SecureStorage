using System;

namespace GroveGames.SecureStorage.Unity
{
    public sealed class SecureStorage : ISecureStorage
    {
        private readonly ISecureStore _store;

        public SecureStorage()
        {
            _store = CreateStore();
        }

        public bool TryRead(string key, out string value)
        {
            ValidateKey(key);
            return _store.TryRead(key, out value);
        }

        public bool Write(string key, string value)
        {
            ValidateKey(key);

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return _store.Write(key, value);
        }

        public bool Delete(string key)
        {
            ValidateKey(key);
            return _store.Delete(key);
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }
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
