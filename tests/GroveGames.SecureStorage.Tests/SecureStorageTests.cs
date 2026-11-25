namespace GroveGames.SecureStorage.Tests;

public sealed class SecureStorageTests
{
    [Fact]
    public void Constructor_NullStore_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new SecureStorage(null!));
    }

    [Fact]
    public void Write_ThenRead_ReturnsValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        var written = storage.Write("token", "value");
        var result = storage.Read("token", out var value);

        Assert.True(written);
        Assert.Equal(SecureStorageReadResult.Found, result);
        Assert.Equal("value", value);
    }

    [Fact]
    public void Read_MissingKey_ReturnsMissing()
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        var result = storage.Read("missing", out var value);

        Assert.Equal(SecureStorageReadResult.Missing, result);
        Assert.Null(value);
    }

    [Fact]
    public void Write_ExistingKey_ReplacesValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());
        storage.Write("token", "first");

        storage.Write("token", "second");
        storage.Read("token", out var value);

        Assert.Equal("second", value);
    }

    [Fact]
    public void Delete_ExistingKey_RemovesValue()
    {
        var storage = new SecureStorage(new InMemorySecureStore());
        storage.Write("token", "value");

        var deleted = storage.Delete("token");
        var result = storage.Read("token", out _);

        Assert.True(deleted);
        Assert.Equal(SecureStorageReadResult.Missing, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Read_NullOrEmptyKey_Throws(string? key)
    {
        var storage = new SecureStorage(new InMemorySecureStore());

        Assert.ThrowsAny<ArgumentException>(() => storage.Read(key!, out _));
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
        var storage = new SecureStorage(new FakeRejectingStore());

        var written = storage.Write("token", "value");

        Assert.False(written);
    }

    [Fact]
    public void Read_StoreFails_ReturnsFailed()
    {
        var storage = new SecureStorage(new FakeRejectingStore());

        var result = storage.Read("token", out var value);

        Assert.Equal(SecureStorageReadResult.Failed, result);
        Assert.Null(value);
    }

    [Fact]
    public void Delete_StoreRejects_ReturnsFalse()
    {
        var storage = new SecureStorage(new FakeRejectingStore());

        var deleted = storage.Delete("token");

        Assert.False(deleted);
    }

    private sealed class FakeRejectingStore : ISecureStore
    {
        public SecureStorageReadResult Read(string key, out string? value)
        {
            value = null;
            return SecureStorageReadResult.Failed;
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
