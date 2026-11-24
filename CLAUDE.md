# CLAUDE.md

## Package

- The core is the `GroveGames.SecureStorage` NuGet package at `src/GroveGames.SecureStorage` (`net10.0`, `net9.0`, `net8.0`, `netstandard2.1`). It holds `ISecureStorage`, `ISecureStore`, `SecureStorage` (key and value validation over an `ISecureStore`) and `InMemorySecureStore`. Tests are in `tests/GroveGames.SecureStorage.Tests` (xUnit v3).
- The Unity package at `src/GroveGames.SecureStorage.Unity/Packages/com.grovegames.securestorage` references `GroveGames.SecureStorage.dll`, installed with NuGetForUnity. Its `ISecureStorage` extends the core interface so existing Unity code keeps compiling. There is no Godot addon.
- `SecureStorage` picks the platform store at compile time: `IosSecureStore` (Keychain, `kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly`), `AndroidSecureStore` (AES-GCM with a Keystore key), and `PlayerPrefsSecureStore` in the editor and on other platforms.
- Native code lives in `Plugins/iOS` and `Plugins/Android`. `AndroidKeepRule` keeps the Java class from being stripped.
- The GroveGames.DependencyInjection integration in `Integrations/GroveGames.DependencyInjection` compiles only when that package is installed and adds `AddSecureStorage()`.

## Code Rules

- No comments.
- Unity code uses block namespaces and C# 9.
- Every file and folder in the package needs a `.meta` with a 32-character lowercase hex GUID. Existing asmdef GUIDs must not change, because projects reference them.
- Tests are EditMode tests in `Tests/Editor`, named `MethodName_Condition_Expected`.

## Agent Skills

- The rules for game code that uses this package are an agent skill. Its source is `skills/grovegames-securestorage/`: `SKILL.md` holds the rules that apply everywhere, `unity.md` holds only what differs in Unity. It is optional.
- The release writes the skill the Unity package ships, the same way it copies the README and the license: `SKILL.md` followed by `unity.md`, when there is one. Never edit the copy under `Skills~/`; the next release replaces it.
- The NuGet package ships `skills/grovegames-securestorage/SKILL.md` as it is.
