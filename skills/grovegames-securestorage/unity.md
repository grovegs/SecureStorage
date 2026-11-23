## Unity

- Register with `builder.AddSecureStorage()` in the root installer. It picks the Keychain on iOS, the Keystore on Android and `PlayerPrefs` in the Editor and elsewhere.
- Values in the Editor are not protected. Test real protection on a device.
- An asmdef that overrides references lists `GroveGames.SecureStorage.dll` in its precompiled references.
