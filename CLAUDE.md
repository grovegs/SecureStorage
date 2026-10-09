# CLAUDE.md

## Package

- Unity-only package at `src/GroveGames.SecureStorage.Unity/Packages/com.grovegames.securestorage`. There is no .NET core or Godot addon, so CI has only the format and release workflows, and releases publish no NuGet package.
- `SecureStorage` picks the platform store at compile time: `IosSecureStore` (Keychain, `kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`), `AndroidSecureStore` (AES-GCM with a Keystore key), and `PlayerPrefsSecureStore` in the editor and on other platforms.
- Native code lives in `Plugins/iOS` and `Plugins/Android`. `AndroidKeepRule` keeps the Java class from being stripped.
- The GroveGames.DependencyInjection integration in `Integrations/GroveGames.DependencyInjection` compiles only when that package is installed and adds `AddSecureStorage()`.

## Code Rules

- No comments.
- Unity code uses block namespaces and C# 9.
- Every file and folder in the package needs a `.meta` with a 32-character lowercase hex GUID. Existing asmdef GUIDs must not change, because projects reference them.
- Tests are EditMode tests in `Tests/Editor`, named `MethodName_Condition_Expected`.
