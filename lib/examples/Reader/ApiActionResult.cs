using System;

namespace func {
    public static class ApiActionResultExtensions {
        public static ApiAction<Result<R>> MapAR<T,R>(this ApiAction<Result<T>> action, Func<T,R> f)
            => action.Map(tResult => tResult.Map(f));

        public static ApiAction<Result<R>> BindAR<T,R>(this ApiAction<Result<T>> action, Func<T,ApiAction<Result<R>>> f)
            => api
            => action.Run(api).Bind(t => f(t)(api));

        public static ApiAction<Result<T>> AsApiActionResult<T>(this T obj)
            => obj.AsResult().AsApiAction();

        public static ApiAction<Result<R>> ApplyAR<T,R>(this ApiAction<Result<T>> action, ApiAction<Result<Func<T,R>>> f) 
            => api
            => f.Run(api).Apply(action.Run(api));
    }
}