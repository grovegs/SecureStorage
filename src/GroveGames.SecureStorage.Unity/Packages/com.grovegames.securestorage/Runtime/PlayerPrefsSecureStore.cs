using UnityEngine;

namespace GroveGames.SecureStorage.Unity
{
    internal sealed class PlayerPrefsSecureStore : ISecureStore
    {
        private const string KeyPrefix = "GroveGames.SecureStorage.";

        public bool TryRead(string key, out string value)
        {
            var prefsKey = KeyPrefix + key;

            if (!PlayerPrefs.HasKey(prefsKey))
            {
                value = null;
                return false;
            }

            value = PlayerPrefs.GetString(prefsKey);
            return true;
        }

        public bool Write(string key, string value)
        {
            PlayerPrefs.SetString(KeyPrefix + key, value);
            PlayerPrefs.Save();
            return true;
        }

        public bool Delete(string key)
        {
            PlayerPrefs.DeleteKey(KeyPrefix + key);
            PlayerPrefs.Save();
            return true;
        }
    }
}
