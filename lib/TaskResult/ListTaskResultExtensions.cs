using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace func {
    
    public static class TaskResultListExtensions {
        private static IEnumerable<T> Append<T>(IEnumerable<T> list, T item) => list.Append(item);

        private static Func<IEnumerable<T>, Func<T, IEnumerable<T>>> CurriedAppend<T>() => l => i => Append(l, i);

        public static Task<Result<IEnumerable<TResult>>> TraverseTaskResultM<T, TResult>(this IEnumerable<T> list, Func<T, Task<Result<TResult>>> f)
        {
            var initialState = Enumerable.Empty<TResult>().AsResult().AsTask();

            Task<Result<IEnumerable<TResult>>> Aggregator(Task<Result<IEnumerable<TResult>>> state, T item)
            {
                return state
                        .BindTaskResult(l => f(item)
                            .BindTaskResult(i => Append(l, i)
                                .AsResult().AsTask()));
            }

            return list.Aggregate(initialState, Aggregator);
        }

        public static Task<Result<IEnumerable<TResult>>> TraverseTaskResultA<T, TResult>(this IEnumerable<T> list, Func<T, Task<Result<TResult>>> f)
        {
            var initialState = Enumerable.Empty<TResult>().AsResult().AsTask();

            Task<Result<IEnumerable<TResult>>> Aggregator(Task<Result<IEnumerable<TResult>>> state, T item)
            {
                var funcToApply = CurriedAppend<TResult>().AsTaskResult();
                return funcToApply.ApplyTaskResult(state).ApplyTaskResult(f(item));
            }
            
            return list.Aggregate(initialState, Aggregator);
        }
    }
}