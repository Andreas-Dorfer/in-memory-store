using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AD.InMemoryStore.Tests
{
    static class Generators
    {
        class ValueTupleArbitrary<T1, T2> : Arbitrary<(T1, T2)>
        {
            static (T1, T2) ToValueTuple(Tuple<T1, T2> tuple) => (tuple.Item1, tuple.Item2);

            public override Gen<(T1, T2)> Generator =>
                Arb.Generate<Tuple<T1, T2>>().Select(ToValueTuple);

            public override IEnumerable<(T1, T2)> Shrinker((T1, T2) _arg1) =>
                Arb.Shrink(Tuple.Create(_arg1.Item1, _arg1.Item2)).Select(ToValueTuple);
        }

        public static Arbitrary<(T1, T2)> ValueTuple2<T1, T2>() => new ValueTupleArbitrary<T1, T2>();
    }
}
