using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public static class ListApiActionResultExtensions {
        private static IEnumerable<T> Append<T>(IEnumerable<T> list, T item) => list.Append(item);

        private static Func<IEnumerable<T>, Func<T, IEnumerable<T>>> CurriedAppend<T>() => l => i => Append(l, i);

        public static ApiAction<Result<IEnumerable<R>>> TraverseA<T, R>(this IEnumerable<T> list, Func<T, ApiAction<Result<R>>> f) 
            => list.Aggregate(Enumerable.Empty<R>().AsApiActionResult(),
                (s, i) => CurriedAppend<R>()
                    .AsApiActionResult()
                        .ApplyAR(s)
                        .ApplyAR(f(i)));

        public static ApiAction<Result<IEnumerable<R>>> TraverseAWithLogging<T, R>(this IEnumerable<T> list, Action<IEnumerable<Error>> log, Func<T, ApiAction<Result<R>>> f) 
            => list.Aggregate(Enumerable.Empty<R>().AsApiActionResult(), 
                (s, i) => f(i).Either(
                     onSuccess: h => CurriedAppend<R>().AsApiActionResult().ApplyAR(s).ApplyAR(h.AsApiActionResult()),
                     onFailure: errs =>
                     {
                         log(errs);
                         return s;
                     }
                 ));
    }    
}