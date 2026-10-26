# GroveGames.SecureStorage

Key-value storage for .NET and Unity, backed by the iOS Keychain and the Android Keystore.

[![NuGet](https://img.shields.io/nuget/v/GroveGames.SecureStorage)](https://www.nuget.org/packages/GroveGames.SecureStorage)
[![Build Status](https://github.com/grovegs/SecureStorage/actions/workflows/release.yml/badge.svg)](https://github.com/grovegs/SecureStorage/actions/workflows/release.yml)
[![Latest Release](https://img.shields.io/github/v/release/grovegs/SecureStorage)](https://github.com/grovegs/SecureStorage/releases/latest)

---

## Features

- **Core**: `ISecureStorage` and `SecureStorage` in the `GroveGames.SecureStorage` NuGet package validate keys and values and delegate to an `ISecureStore`. `InMemorySecureStore` is included for tests and tools.
- **iOS**: Values are stored as Keychain items, accessible after the first unlock and never synced or restored to another device.
- **Android**: Values are encrypted with AES-GCM using a key held in the Android Keystore, and stored in private shared preferences.
- **Editor and other platforms**: Values are stored in `PlayerPrefs`, so the same code runs everywhere.
- **Dependency injection**: `AddSecureStorage()` registers `ISecureStorage` with [GroveGames.DependencyInjection](https://github.com/grovegs/DependencyInjection).

## Installation

### .NET

```bash
dotnet add package GroveGames.SecureStorage
```

### Unity

Install the core through [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) (`GroveGames.SecureStorage`), then add the package to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.grovegames.securestorage": "https://github.com/grovegs/SecureStorage.git?path=src/GroveGames.SecureStorage.Unity/Packages/com.grovegames.securestorage"
  }
}
```

## Usage

In Unity, `SecureStorage` picks the Keychain, the Keystore or `PlayerPrefs` for the current platform:

```csharp
ISecureStorage storage = new SecureStorage();

storage.Write("session-token", token);

if (storage.TryRead("session-token", out var value))
{
}

storage.Delete("session-token");
```

Keys cannot be empty and values cannot be null. `Write` and `Delete` return `false` when the platform store rejects the change.

With GroveGames.DependencyInjection installed, register it in an installer and inject `ISecureStorage`:

```csharp
builder.AddSecureStorage();
```

In .NET, pass a store to the core `SecureStorage`:

```csharp
var storage = new GroveGames.SecureStorage.SecureStorage(new InMemorySecureStore());
```

Values stay on the device. Restoring a backup to a new device restores the app's files but not these values, so data that depends on them must be recreatable.

## Testing

```bash
dotnet test
```

The Unity EditMode tests run from `sandbox/UnityApplication` with the Unity Test Runner.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
