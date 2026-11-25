#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GroveGames.SecureStorage.Unity
{
    internal sealed class IosSecureStore : ISecureStore
    {
        private const int MissingStatus = 1;

        [DllImport("__Internal")]
        private static extern IntPtr GroveGamesSecureStorage_Read(byte[] key, int keyLength, out int valueLength, out int status);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GroveGamesSecureStorage_Write(byte[] key, int keyLength, byte[] value, int valueLength);

        [DllImport("__Internal")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GroveGamesSecureStorage_Delete(byte[] key, int keyLength);

        [DllImport("__Internal")]
        private static extern void GroveGamesSecureStorage_Free(IntPtr buffer);

        public SecureStorageReadResult Read(string key, out string value)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var buffer = GroveGamesSecureStorage_Read(keyBytes, keyBytes.Length, out var valueLength, out var status);

            if (buffer == IntPtr.Zero)
            {
                value = null;
                return status == MissingStatus ? SecureStorageReadResult.Missing : SecureStorageReadResult.Failed;
            }

            try
            {
                var valueBytes = new byte[valueLength];
                Marshal.Copy(buffer, valueBytes, 0, valueLength);
                value = Encoding.UTF8.GetString(valueBytes);
                return SecureStorageReadResult.Found;
            }
            finally
            {
                GroveGamesSecureStorage_Free(buffer);
            }
        }

        public bool Write(string key, string value)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return GroveGamesSecureStorage_Write(keyBytes, keyBytes.Length, valueBytes, valueBytes.Length);
        }

        public bool Delete(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            return GroveGamesSecureStorage_Delete(keyBytes, keyBytes.Length);
        }
    }
}
#endif
