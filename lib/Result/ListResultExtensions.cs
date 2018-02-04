using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public static class ListResultExtensions {
        private static IEnumerable<T> Append<T>(IEnumerable<T> list, T item) => list.Append(item);

        private static Func<IEnumerable<T>, Func<T, IEnumerable<T>>> CurriedAppend<T>() => l => i => Append(l, i);

        public static Result<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> list, Func<T, Result<R>> f) => 
            list.Aggregate(Enumerable.Empty<R>().AsResult(), (s, i) 
                => CurriedAppend<R>()
                    .AsResult()
                        .Apply(s)
                        .Apply(f(i)));
    }
}