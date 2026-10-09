using System;
using NUnit.Framework;

namespace GroveGames.SecureStorage.Unity.Editor.Tests
{
    public sealed class SecureStorageTests
    {
        private SecureStorage _storage;
        private string _key;

        [SetUp]
        public void SetUp()
        {
            _storage = new SecureStorage();
            _key = "GroveGames.SecureStorage.Tests." + Guid.NewGuid().ToString("N");
        }

        [TearDown]
        public void TearDown()
        {
            _storage.Delete(_key);
        }

        [Test]
        public void TryRead_MissingKey_ReturnsFalse()
        {
            Assert.IsFalse(_storage.TryRead(_key, out _));
        }

        [Test]
        public void Write_ThenTryRead_ReturnsValue()
        {
            Assert.IsTrue(_storage.Write(_key, "value"));

            Assert.IsTrue(_storage.TryRead(_key, out var value));
            Assert.AreEqual("value", value);
        }

        [Test]
        public void Write_ExistingKey_ReplacesValue()
        {
            _storage.Write(_key, "first");
            _storage.Write(_key, "second");

            _storage.TryRead(_key, out var value);
            Assert.AreEqual("second", value);
        }

        [Test]
        public void Delete_StoredKey_RemovesValue()
        {
            _storage.Write(_key, "value");

            Assert.IsTrue(_storage.Delete(_key));
            Assert.IsFalse(_storage.TryRead(_key, out _));
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
