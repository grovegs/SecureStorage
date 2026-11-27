#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace GroveGames.SecureStorage.Unity
{
    internal sealed class AndroidSecureStore : ISecureStore
    {
        private const string JavaClassName = "com.grovegames.securestorage.SecureStorage";

        private readonly AndroidJavaObject _storage;

        public AndroidSecureStore()
        {
            try
            {
                using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                using var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                _storage = new AndroidJavaObject(JavaClassName, activity);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                _storage = null;
            }
        }

        public SecureStorageReadResult Read(string key, out string value)
        {
            if (_storage == null)
            {
                value = null;
                return SecureStorageReadResult.Failed;
            }

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
            return _storage != null && _storage.Call<bool>("write", key, value);
        }

        public bool Delete(string key)
        {
            return _storage != null && _storage.Call<bool>("delete", key);
        }
    }
}
#endif
