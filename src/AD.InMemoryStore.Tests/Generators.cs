using FsCheck;

namespace AD.InMemoryStore.Tests;

static class Generators
{
    class ValueTupleArbitrary<T1, T2> : Arbitrary<(T1, T2)>
    {
        public override Gen<(T1, T2)> Generator =>
            Arb.Generate<Tuple<T1, T2>>().Select(_ => _.ToValueTuple());

        public override IEnumerable<(T1, T2)> Shrinker((T1, T2) arg) =>
            Arb.Shrink(arg.ToTuple()).Select(_ => _.ToValueTuple());
    }

    public static Arbitrary<(T1, T2)> ValueTuple2<T1, T2>() => new ValueTupleArbitrary<T1, T2>();
}
