﻿namespace AD.InMemoryStore.Tests;

[TestClass]
public sealed class VersionTests
{
    readonly KeyValueStore<int, string> store = new();

    [TestMethod("To string and parse round trip")]
    public void ToStringParse()
    {
        var expected = store.Add(1, "A");
        var actual = Version.Parse(expected.ToString());
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod()]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("1")]
    [DataRow("\"1")]
    [DataRow("1\"")]
    [DataRow("a")]
    [DataRow("\"a\"")]
    [DataRow("\"-1\"")]
    [ExpectedException(typeof(ArgumentException))]
    public void InvalidVersionParse(string text)
    {
        Version.Parse(text);
    }
}
