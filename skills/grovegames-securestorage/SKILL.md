---
name: grovegames-securestorage
description: Rules for storing secrets on the device in game code with GroveGames.SecureStorage. Use when writing or reviewing code that saves or reads a token, key or other value that must not be readable or editable by the player.
---

# Using GroveGames.SecureStorage

Rules for game code that uses this package. They hold in every game. The API is in the package's `README.md`.

- Take `ISecureStorage` through the constructor. Never put a secret in a save file, in plain preferences or in code.
- Use it for small secrets only: tokens and keys. Game progress belongs in the save system.
- Check the result of `Write` and `Delete`; `false` means the platform store rejected the change.
- Read with `TryRead` and handle a missing value. Values do not survive a restore to a new device, so anything that depends on one must be recreatable.
- Keys are constants in the class that owns the value. Never log a value.
- In tests, use the core `SecureStorage` with `InMemorySecureStore`.
