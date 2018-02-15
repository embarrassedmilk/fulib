using System;
using System.Threading.Tasks;

namespace Fulib
{
    public static class LinqTaskResultExtensions {
        public static Task<Result<R>> Select<T,R>(this Task<Result<T>> taskResult, Func<T, R> f)
            => taskResult.MapTaskResult(f);

        public static Task<Result<RR>> SelectMany<T,R,RR>(this Task<Result<T>> taskResult, Func<T,Task<Result<R>>> bind, Func<T,R,RR> project)
            => taskResult.BindTaskResult(t => bind(t).MapTaskResult(r => project(t,r)));
    }
}