## Unity

- Register with `builder.AddSecureStorage()` in the root installer. It picks the Keychain on iOS, the Keystore on Android and `PlayerPrefs` in the Editor and elsewhere. `PlayerPrefs` is plain text, so on desktop and WebGL a stored value is not secret.
- Values in the Editor are not protected. Test real protection on a device.
- An asmdef that overrides references lists `GroveGames.SecureStorage.dll` in its precompiled references.
