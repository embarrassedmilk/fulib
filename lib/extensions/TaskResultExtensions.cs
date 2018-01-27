using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class TaskResultExtensions {
        public static Task<Result<B>> MapLocal<A,B>(this Task<Result<A>> task, Func<A,B> f) => task.Map(r => r.Map(f));

        public static Task<Result<A>> AsTaskResult<A>(this A obj) => obj.AsResult().AsTask();

        public static Task<Result<TResult>> ApplyTaskResult<T, TResult>(this Task<Result<Func<T, TResult>>> f, Task<Result<T>> taskResult) => 
            f.Bind(fResult => taskResult.Map(tResult => fResult.Apply(tResult)));

        public static async Task<Result<B>> BindTaskResult<A,B>(this Task<Result<A>> task, Func<A,Task<Result<B>>> f) {
            var taskResult = await task;
            if (!taskResult.IsSuccess) {
                return Result<B>.Failure(taskResult.Errors);
            }
            var result = await f(taskResult.Value);
            return result;
        }

        public static async Task<Result<TResult>> Then<T, TResult>(this Task<Result<T>> task,
            Func<T, Result<TResult>> f)
        {
            var taskResult = await task;

            return taskResult.Match(
                Succ: f,
                Fail: Result<TResult>.Failure
            );
        }

        public static async Task<Result<T>> ThenVoid<T>(this Task<Result<T>> task, Action<T> f)
        {
            var taskResult = await task;

            return taskResult.TryCatchTee(f);
        }
    }
}