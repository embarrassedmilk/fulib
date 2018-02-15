using System;
using System.Collections.Generic;

namespace Fulib
{
    public static class ReaderResultExtensions
    {
        public static Reader<E, Result<R>> MapAR<E, T, R>(this Reader<E, Result<T>> action, Func<T, R> f)
            => action.Map(tResult => tResult.Map(f));

        public static Reader<E, Result<R>> BindAR<E, T, R>(this Reader<E, Result<T>> action, Func<T, Reader<E, Result<R>>> f)
            => api
            => action.Run(api).Bind(t => f(t)(api));

        public static Reader<E, Result<T>> AsReaderResult<E, T>(this T obj)
            => obj.AsResult().AsReader<E, Result<T>>();

        public static Reader<E, Result<R>> ApplyAR<E, T, R>(this Reader<E, Result<Func<T, R>>> f, Reader<E, Result<T>> action)
            => api
            => f.Run(api).Apply(action.Run(api));

        public static Reader<E, Result<R>> Either<E, T, R>(this Reader<E, Result<T>> action, Func<T, Reader<E, Result<R>>> onSuccess, Func<IReadOnlyCollection<Error>, Reader<E, Result<R>>> onFailure)
            => api
            => action
                .Run(api)
                .EitherWithReader(onSuccess, onFailure)
                .Run(api);

        public static Reader<E, Result<IEnumerable<R>>> ThenTraverseApplicative<E, T, R>(this Reader<E, Result<IEnumerable<T>>> action, Func<T, Reader<E, Result<R>>> f)
            => action.BindAR(items => items.TraverseA(f));

        public static Reader<E, Result<IEnumerable<R>>> ThenTraverseApplicativeWithLogs<E, T, R>(this Reader<E, Result<IEnumerable<T>>> action, Func<T, Reader<E, Result<R>>> f, Action<IEnumerable<Error>> log)
            => action.BindAR(items => items.TraverseAWithLogging(log, f));
    }
}