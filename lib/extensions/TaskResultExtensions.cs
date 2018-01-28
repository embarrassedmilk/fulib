using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class TaskResultExtensions {
        public static Task<Result<TResult>> MapLocal<T,TResult>(this Task<Result<T>> task, Func<T,TResult> f) => task.Map(r => r.Map(f));

        public static Task<Result<T>> AsTaskResult<T>(this T obj) => obj.AsResult().AsTask();

        public static async Task<Result<TResult>> ApplyTaskResult<T, TResult>(this Task<Result<Func<T, TResult>>> f, Task<Result<T>> taskResult) 
        {
            var fResult = await f;
            var tResult = await taskResult;

            return fResult.Apply(tResult);
        }

        public static async Task<Result<TResult>> BindTaskResult<T,TResult>(this Task<Result<T>> task, Func<T,Task<Result<TResult>>> f) {
            var taskResult = await task;

            return await taskResult.MatchAsync(
                Succ: val => f(val),
                Fail: Result<TResult>.Failure
            );
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