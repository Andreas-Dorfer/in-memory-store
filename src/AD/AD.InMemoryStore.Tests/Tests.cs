using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AD.InMemoryStore.Tests
{
    [TestClass]
    public class Tests
    {
        static Tests()
        {
            Arb.Register(typeof(Generators));
        }

        [TestMethod]
        public void Add() =>
            Prop.ForAll<Dictionary<Guid, string>>(expectedValues =>
            {
                InMemoryStore<Guid, string> sut = new();

                var actualValues = DoInParallel(expectedValues, v => sut.Add(v.Key, v.Value));

                foreach (var ((_, expected), (actual, _)) in expectedValues.Zip(actualValues))
                {
                    Assert.AreEqual(expected, actual);
                }
            }).QuickCheckThrowOnFailure();

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException<Guid>))]
        public void Cannot_add_a_duplicate_key()
        {
            InMemoryStore<Guid, string> sut = new();
            var key = Guid.NewGuid();
            sut.Add(key, "A");
            sut.Add(key, "B");
        }

        [TestMethod]
        public void Get() =>
            Prop.ForAll<Dictionary<Guid, string>>(values =>
            {
                InMemoryStore<Guid, string> sut = new();
                var expectedValues = DoInParallel(values, v => sut.Add(v.Key, v.Value));

                var actualValues = DoInParallel(values, v => sut.Get(v.Key));

                foreach (var (expected, actual) in expectedValues.Zip(actualValues))
                {
                    Assert.AreEqual(expected, actual);
                }
            }).QuickCheckThrowOnFailure();

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException<Guid>))]
        public void Cannot_get_an_unknown_value()
        {
            InMemoryStore<Guid, string> sut = new();
            var unknownId = Guid.NewGuid();
            sut.Get(unknownId);
        }

        [TestMethod]
        public void GetAll() =>
            Prop.ForAll<Dictionary<Guid, string>>(values =>
            {
                InMemoryStore<Guid, string> sut = new();
                var expectedValues = DoInParallel(values, v =>
                {
                    var result = sut.Add(v.Key, v.Value);
                    return (v.Key, result.Value, result.Version);
                }).OrderBy(_ => _.Key);

                var actualValues = sut.GetAll().OrderBy(_ => _.Key);

                foreach (var (expected, actual) in expectedValues.Zip(actualValues))
                {
                    Assert.AreEqual(expected, actual);
                }
            }).QuickCheckThrowOnFailure();

        [TestMethod]
        public void Update() =>
            Prop.ForAll<Dictionary<Guid, (string InitialValue, string UpdateValue)>>(values =>
            {
                InMemoryStore<Guid, string> sut = new();
                var initialValues = DoInParallel(values, v => sut.Add(v.Key, v.Value.InitialValue));

                var updateValues = values.Zip(initialValues, (v, iv) => (v.Key, v.Value.UpdateValue, iv.Version));
                Parallel.ForEach(updateValues, value =>
                {
                    var actual = sut.Update(value.Key, value.UpdateValue, value.Version);
                    Assert.AreEqual(values[value.Key].UpdateValue, actual.Value);
                    Assert.AreNotEqual(value.Version, actual.Version);

                    var afterUpdate = sut.Get(value.Key);
                    Assert.AreEqual(actual, afterUpdate);
                });
            }).QuickCheckThrowOnFailure();

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException<Guid>))]
        public void Cannot_update_an_unknown_value()
        {
            InMemoryStore<Guid, string> sut = new();
            var unknownId = Guid.NewGuid();
            sut.Update(unknownId, "X");
        }

        [TestMethod]
        [ExpectedException(typeof(ConcurrencyException<Guid>))]
        public void Update_cheks_version()
        {
            InMemoryStore<Guid, string> sut = new();
            var key = Guid.NewGuid();
            var (_, initialVersion) = sut.Add(key, "A");
            sut.Update(key, "B", initialVersion);
            sut.Update(key, "C", initialVersion);
        }

        [TestMethod]
        public void Update_with_no_version_check()
        {
            InMemoryStore<Guid, string> sut = new();
            var key = Guid.NewGuid();
            var (_, initialVersion) = sut.Add(key, "A");
            sut.Update(key, "B", initialVersion);
            sut.Update(key, "C");

            var (afterUpdate, _) = sut.Get(key);
            Assert.AreEqual("C", afterUpdate);
        }


        static T2[] DoInParallel<T1, T2>(IEnumerable<T1> values, Func<T1, T2> function) =>
            Task.WhenAll(values.Select(value => Task.Run(() => function(value)))).Result;
    }
}
