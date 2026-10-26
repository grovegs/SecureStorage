using System.Diagnostics.CodeAnalysis;

namespace GroveGames.SecureStorage.Tests;

public sealed class SecureStorageTests
{
    [Fact]
    public void Constructor_NullStore_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new SecureStorage(null!));
    }

    [Fact]
    public void Write_ThenTryRead_ReturnsValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        var written = storage.Write("token", "value");
        var found = storage.TryRead("token", out var value);

        Assert.True(written);
        Assert.True(found);
        Assert.Equal("value", value);
    }

    [Fact]
    public void TryRead_MissingKey_ReturnsFalse()
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        var found = storage.TryRead("missing", out var value);

        Assert.False(found);
        Assert.Null(value);
    }

    [Fact]
    public void Write_ExistingKey_ReplacesValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());
        storage.Write("token", "first");

        storage.Write("token", "second");
        storage.TryRead("token", out var value);

        Assert.Equal("second", value);
    }

    [Fact]
    public void Delete_ExistingKey_RemovesValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());
        storage.Write("token", "value");

        var deleted = storage.Delete("token");
        var found = storage.TryRead("token", out _);

        Assert.True(deleted);
        Assert.False(found);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryRead_NullOrEmptyKey_Throws(string? key)
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        Assert.ThrowsAny<ArgumentException>(() => storage.TryRead(key!, out _));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Write_NullOrEmptyKey_Throws(string? key)
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        Assert.ThrowsAny<ArgumentException>(() => storage.Write(key!, "value"));
    }

    [Fact]
    public void Write_NullValue_Throws()
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        Assert.Throws<ArgumentNullException>(() => storage.Write("token", null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Delete_NullOrEmptyKey_Throws(string? key)
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        Assert.ThrowsAny<ArgumentException>(() => storage.Delete(key!));
    }

    [Fact]
    public void Write_StoreRejects_ReturnsFalse()
    {
        var storage = new SecureStorage(new TestRejectingStore());

        var written = storage.Write("token", "value");

        Assert.False(written);
    }

    [Fact]
    public void Delete_StoreRejects_ReturnsFalse()
    {
        var storage = new SecureStorage(new TestRejectingStore());

        var deleted = storage.Delete("token");

        Assert.False(deleted);
    }

    private sealed class TestRejectingStore : ISecureStore
    {
        public bool TryRead(string key, [NotNullWhen(true)] out string? value)
        {
            value = null;
            return false;
        }

        public bool Write(string key, string value)
        {
            return false;
        }

        public bool Delete(string key)
        {
            return false;
        }
    }
}
