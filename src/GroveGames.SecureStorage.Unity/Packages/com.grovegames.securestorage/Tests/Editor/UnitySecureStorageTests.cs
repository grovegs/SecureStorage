using System;
using NUnit.Framework;

namespace GroveGames.SecureStorage.Unity.Editor.Tests
{
    public sealed class UnitySecureStorageTests
    {
        private UnitySecureStorage _storage;
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _storage = new UnitySecureStorage();
            _key = "GroveGames.SecureStorage.Tests." + Guid.NewGuid().ToString("N");
        }

        [TearDown]
        public void TearDown()
        {
            _storage.Delete(_key);
        }

        [Test]
        public void Read_MissingKey_ReturnsMissing()
        {
            Assert.AreEqual(SecureStorageReadResult.Missing, _storage.Read(_key, out _));
        }

        [Test]
        public void Write_ThenRead_ReturnsValue()
        {
            Assert.IsTrue(_storage.Write(_key, "value"));

            Assert.AreEqual(SecureStorageReadResult.Found, _storage.Read(_key, out var value));
            Assert.AreEqual("value", value);
        }

        [Test]
        public void Write_ExistingKey_ReplacesValue()
        {
            _storage.Write(_key, "first");
            _storage.Write(_key, "second");

            _storage.Read(_key, out var value);
            Assert.AreEqual("second", value);
        }

        [Test]
        public void Delete_StoredKey_RemovesValue()
        {
            _storage.Write(_key, "value");

            Assert.IsTrue(_storage.Delete(_key));
            Assert.AreEqual(SecureStorageReadResult.Missing, _storage.Read(_key, out _));
        }

        [Test]
        public void Write_EmptyKey_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _storage.Write(string.Empty, "value"));
        }

        [Test]
        public void Write_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _storage.Write(_key, null));
        }
    }
}
