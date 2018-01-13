using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class TaskResultExtensions {
        public static Task<Result<B>> MapLocal<A,B>(this Task<Result<A>> task, Func<A,B> f) => task.Map(r => r.Map(f));

        public static Task<Result<A>> AsTaskResult<A>(this A obj) => obj.AsResult().AsTask();

        public static Task<Result<B>> Apply<A,B>(this Task<Result<A>> task, Task<Result<Func<A,B>>> f) => f.Bind(fResult => task.Map(tResult => tResult.Apply(fResult)));

        public static async Task<Result<B>> BindLocal<A,B>(this Task<Result<A>> task, Func<A,Task<Result<B>>> f) {
            var taskResult = await task;
            if (!taskResult.IsSuccess) {
                return Result<B>.Failure(taskResult.Errors);
            }
            var result = await f(taskResult.Value);
            return result;
        }
    }
}