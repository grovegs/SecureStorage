#if UNITY_ANDROID
using System.IO;

using UnityEditor.Android;

namespace GroveGames.SecureStorage.Unity.Editor
{
    internal sealed class AndroidKeepRule : IPostGenerateGradleAndroidProject
    {
        private const string Rule = "-keep class com.grovegames.securestorage.** { *; }";

        public int callbackOrder => 0;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var rulesPath = Path.Combine(path, "proguard-unity.txt");
            var rules = File.Exists(rulesPath) ? File.ReadAllText(rulesPath) : string.Empty;

            if (rules.Contains(Rule))
            {
                return;
            }

            File.AppendAllText(rulesPath, (rules.Length == 0 || rules.EndsWith("\n") ? string.Empty : "\n") + Rule + "\n");
        }
    }
}
#endif
