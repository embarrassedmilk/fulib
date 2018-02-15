using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fulib
{
    public static class TaskResultListExtensions {
        private static IEnumerable<T> Append<T>(IEnumerable<T> list, T item) => list.Append(item);

        private static Func<IEnumerable<T>, Func<T, IEnumerable<T>>> CurriedAppend<T>() => l => i => Append(l, i);

        public static Task<Result<IEnumerable<TResult>>> TraverseTaskResultM<T, TResult>(this IEnumerable<T> list, Func<T, Task<Result<TResult>>> f)
            => list.Aggregate(Enumerable.Empty<TResult>().AsTaskResult(), (s, i) => s.BindTaskResult(l => f(i).BindTaskResult(h => Append(l, h).AsResult().AsTask())));

        public static Task<Result<IEnumerable<TResult>>> TraverseTaskResultA<T, TResult>(this IEnumerable<T> list, Func<T, Task<Result<TResult>>> f)
            => list.Aggregate(Enumerable.Empty<TResult>().AsTaskResult(), (s, i) => CurriedAppend<TResult>().AsTaskResult().ApplyTaskResult(s).ApplyTaskResult(f(i)));
    }
}