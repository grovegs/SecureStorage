#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace GroveGames.SecureStorage.Unity
{
    internal sealed class AndroidSecureStore : ISecureStore
    {
        private const string JavaClassName = "com.grovegames.securestorage.SecureStorage";

        private readonly AndroidJavaObject _storage;

        public AndroidSecureStore()
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            _storage = new AndroidJavaObject(JavaClassName, activity);
        }

        public SecureStorageReadResult Read(string key, out string value)
        {
            try
            {
                value = _storage.Call<string>("read", key);
            }
            catch (AndroidJavaException)
            {
                value = null;
                return SecureStorageReadResult.Failed;
            }

            return value != null ? SecureStorageReadResult.Found : SecureStorageReadResult.Missing;
        }

        public bool Write(string key, string value)
        {
            return _storage.Call<bool>("write", key, value);
        }

        public bool Delete(string key)
        {
            return _storage.Call<bool>("delete", key);
        }
    }
}
#endif
