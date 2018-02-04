using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public static class ListApiActionResultExtensions {
        private static IEnumerable<T> Append<T>(IEnumerable<T> list, T item) => list.Append(item);

        private static Func<IEnumerable<T>, Func<T, IEnumerable<T>>> CurriedAppend<T>() => l => i => Append(l, i);

        public static Reader<E,Result<IEnumerable<R>>> TraverseA<E,T,R>(this IEnumerable<T> list, Func<T, Reader<E,Result<R>>> f) 
            => list.Aggregate(Enumerable.Empty<R>().AsReaderResult<E,IEnumerable<R>>(),
                (s, i) => CurriedAppend<R>()
                    .AsReaderResult<E,Func<IEnumerable<R>, Func<R, IEnumerable<R>>>>()
                        .ApplyAR(s)
                        .ApplyAR(f(i)));

        public static Reader<E,Result<IEnumerable<R>>> TraverseAWithLogging<E,T,R>(this IEnumerable<T> list, Action<IEnumerable<Error>> log, Func<T, Reader<E,Result<R>>> f) 
            => list.Aggregate(Enumerable.Empty<R>().AsReaderResult<E,IEnumerable<R>>(),
                (s, i) => f(i).Either(
                     onSuccess: h => CurriedAppend<R>()
                        .AsReaderResult<E,Func<IEnumerable<R>, Func<R, IEnumerable<R>>>>().ApplyAR(s).ApplyAR(h.AsReaderResult<E,R>()),
                     onFailure: errs =>
                     {
                         log(errs);
                         return s;
                     }
                 ));
    }    
}