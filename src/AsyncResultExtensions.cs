using System;
using System.Threading.Tasks;

namespace func {
    public static class AsyncResultExtensions {
        public static Async<Result<B>> MapAR<A,B>(this Async<Result<A>> async, Func<A,B> f) => async.Map(r => r.Map(f));

        public static Async<Result<A>> AsAsyncResult<A>(this A obj) => obj.AsResult().AsAsync();

        public static Async<Result<B>> ApplyAR<A,B>(this Async<Result<A>> async, Async<Result<Func<A,B>>> f) => f.Bind(fResult => async.Map(tResult => tResult.Apply(fResult)));

        public static Async<Result<B>> BindAR<A,B>(this Async<Result<A>> async, Func<A,Async<Result<B>>> f) {
            return new Async<Result<B>>(async () => {
                var asyncResult = await async.Run();
                if (!asyncResult.IsSuccess) {
                    return Result<B>.Failure(asyncResult.Errors);
                }
                var result = await f(asyncResult.Value).Run();
                return result;
            });
            
        }
    }
}