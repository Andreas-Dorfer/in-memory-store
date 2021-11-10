namespace AD.InMemoryStore.Tests;

[TestClass]
public class KeyValueStoreTests
{
    static KeyValueStoreTests()
    {
        Arb.Register(typeof(Generators));
    }

    [TestMethod]
    [ExpectedException(typeof(DuplicateKeyException<Guid>))]
    public void Cannot_add_a_duplicate_key()
    {
        KeyValueStore<Guid, string> sut = new();
        var key = Guid.NewGuid();

        sut.Add(key, "A");
        sut.Add(key, "B");
    }

    [TestMethod]
    public void Get() =>
        Prop.ForAll<Dictionary<Guid, string>>(values =>
        {
            KeyValueStore<Guid, string> sut = new();
            var expectedVersions = DoInParallel(values, v => sut.Add(v.Key, v.Value));
            var expectedValues = values.Zip(expectedVersions, (v, version) => (v.Value, version));

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
        KeyValueStore<Guid, string> sut = new();
        var unknownId = Guid.NewGuid();

        sut.Get(unknownId);
    }

    [TestMethod]
    public void GetAll() =>
        Prop.ForAll<Dictionary<Guid, string>>(values =>
        {
            KeyValueStore<Guid, string> sut = new();
            var expectedValues = DoInParallel(values, v =>
            {
                var version = sut.Add(v.Key, v.Value);
                return (v.Key, v.Value, version);
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
            KeyValueStore<Guid, string> sut = new();
            var initialVersions = DoInParallel(values, v => sut.Add(v.Key, v.Value.InitialValue));

            var updateValues = values.Zip(initialVersions, (v, version) => (v.Key, v.Value.UpdateValue, Version: version));
            Parallel.ForEach(updateValues, value =>
            {
                var updatedVersion = sut.Update(value.Key, value.UpdateValue, value.Version);

                Assert.AreNotEqual(value.Version, updatedVersion);
                var afterUpdate = sut.Get(value.Key);
                Assert.AreEqual((value.UpdateValue, updatedVersion), afterUpdate);
            });
        }).QuickCheckThrowOnFailure();

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException<Guid>))]
    public void Cannot_update_an_unknown_value()
    {
        KeyValueStore<Guid, string> sut = new();
        var unknownId = Guid.NewGuid();

        sut.Update(unknownId, "X");
    }

    [TestMethod]
    [ExpectedException(typeof(VersionMismatchException<Guid>))]
    public void Update_with_version_check()
    {
        KeyValueStore<Guid, string> sut = new();
        var key = Guid.NewGuid();
        var initialVersion = sut.Add(key, "A");

        sut.Update(key, "B", initialVersion);
        sut.Update(key, "C", initialVersion);
    }

    [TestMethod]
    public void Update_with_no_version_check()
    {
        KeyValueStore<Guid, string> sut = new();
        var key = Guid.NewGuid();
        sut.Add(key, "A");
        var expectedValue = "B";

        sut.Update(key, expectedValue);

        var (afterUpdate, _) = sut.Get(key);
        Assert.AreEqual(expectedValue, afterUpdate);
    }

    [TestMethod]
    public void Remove() =>
        Prop.ForAll<Dictionary<Guid, string>>(values =>
        {
            KeyValueStore<Guid, string> sut = new();
            var initialVersions = DoInParallel(values, v => sut.Add(v.Key, v.Value));

            var deleteValues = values.Zip(initialVersions, (v, version) => (v.Key, Version: version));
            Parallel.ForEach(deleteValues, value =>
            {
                sut.Remove(value.Key, value.Version);

                Assert.ThrowsException<KeyNotFoundException<Guid>>(() => sut.Get(value.Key));
            });
        }).QuickCheckThrowOnFailure();

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException<Guid>))]
    public void Cannot_remove_an_unknown_value()
    {
        KeyValueStore<Guid, string> sut = new();
        var unknownId = Guid.NewGuid();

        sut.Remove(unknownId);
    }

    [TestMethod]
    [ExpectedException(typeof(VersionMismatchException<Guid>))]
    public void Remove_with_version_check()
    {
        KeyValueStore<Guid, string> sut = new();
        var key = Guid.NewGuid();
        var initialVersion = sut.Add(key, "A");

        sut.Update(key, "B", initialVersion);
        sut.Remove(key, initialVersion);
    }

    [TestMethod]
    public void Remove_with_no_version_check()
    {
        KeyValueStore<Guid, string> sut = new();
        var key = Guid.NewGuid();
        var initialVersion = sut.Add(key, "A");

        sut.Update(key, "B", initialVersion);
        sut.Remove(key);

        Assert.ThrowsException<KeyNotFoundException<Guid>>(() => sut.Get(key));
    }


    static T2[] DoInParallel<T1, T2>(IEnumerable<T1> values, Func<T1, T2> function) =>
        Task.WhenAll(values.Select(value => Task.Run(() => function(value)))).Result;
}
