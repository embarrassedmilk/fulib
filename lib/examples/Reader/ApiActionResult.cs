using System;
using System.Collections.Generic;

namespace func {
    public static class ApiActionResultExtensions {
        public static ApiAction<Result<R>> MapAR<T,R>(this ApiAction<Result<T>> action, Func<T,R> f)
            => action.Map(tResult => tResult.Map(f));

        public static ApiAction<Result<R>> BindAR<T,R>(this ApiAction<Result<T>> action, Func<T,ApiAction<Result<R>>> f)
            => api
            => action.Run(api).Bind(t => f(t)(api));

        public static ApiAction<Result<T>> AsApiActionResult<T>(this T obj)
            => obj.AsResult().AsApiAction();

        public static ApiAction<Result<R>> ApplyAR<T,R>(this ApiAction<Result<Func<T,R>>> f, ApiAction<Result<T>> action) 
            => api
            => f.Run(api).Apply(action.Run(api));

        public static ApiAction<Result<R>> Either<T,R>(this ApiAction<Result<T>> action, Func<T,ApiAction<Result<R>>> onSuccess, Func<IReadOnlyCollection<Error>, ApiAction<Result<R>>> onFailure)
            => api 
            => action
                .Run(api)
                .Match(onSuccess, onFailure)
                .Run(api);

        public static ApiAction<Result<IEnumerable<R>>> ThenTraverseApplicative<T,R>(this ApiAction<Result<IEnumerable<T>>> action, Func<T,ApiAction<Result<R>>> f)
            => action.BindAR(items => items.TraverseA(f));

        public static ApiAction<Result<IEnumerable<R>>> ThenTraverseApplicativeWithLogs<T,R>(this ApiAction<Result<IEnumerable<T>>> action, Func<T,ApiAction<Result<R>>> f, Action<IEnumerable<Error>> log)
            => action.BindAR(items => items.TraverseAWithLogging(log, f));
    }
}