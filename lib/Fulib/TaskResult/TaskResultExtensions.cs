using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fulib
{
    public static class TaskResultExtensions {
        public static async Task<Result<TResult>> MapTaskResult<T,TResult>(this Task<Result<T>> task, Func<T,TResult> f) {
            try
            {
                var taskResult = await task;
                return taskResult.Map(t => f(t));
            }
            catch (Exception ex)
            {
                return Result<TResult>.Failure(ex);
            }
        }

        public static Task<Result<T>> AsTaskResult<T>(this T obj) => obj.AsResult().AsTask();

        public static async Task<Result<TResult>> ApplyTaskResult<T, TResult>(this Task<Result<Func<T, TResult>>> f, Task<Result<T>> taskResult) 
        {
            try
            {
                var fResult = await f;
                var tResult = await taskResult;

                return fResult.Apply(tResult);
            }
            catch (Exception ex)
            {
                return Result<TResult>.Failure(ex);
            }
        }

        public static async Task<Result<TResult>> BindTaskResult<T,TResult>(this Task<Result<T>> task, Func<T,Task<Result<TResult>>> f)
        {
            try
            {
                var taskResult = await task;

                return await taskResult.MatchAsync(
                    Succ: val => f(val),
                    Fail: Result<TResult>.Failure
                );
            }
            catch (Exception ex)
            {
                return Result<TResult>.Failure(ex);
            }
        }

        public static async Task<Result<T>> TeeTaskResultAsync<T>(this Task<Result<T>> task, Func<T, Task> f)
        {
            try
            {
                var taskResult = await task;
                return await taskResult.MatchTeeAsync(
                    Succ: f,
                    Fail: _ => { }
                );
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex);
            }
        }

        public static async Task<Result<T>> TeeTaskResult<T>(this Task<Result<T>> task, Action<T> f)
        {
            try
            {
                var taskResult = await task;
                return taskResult.MatchTee(
                    Succ: f,
                    Fail: _ => { }
                );
            }
            catch (Exception ex)
            {
                return Result<T>.Failure(ex);
            }
        }

        public static async Task<Result<TResult>> Then<T, TResult>(this Task<Result<T>> task,
            Func<T, Result<TResult>> f)
        {
            try
            {
                var taskResult = await task;

                return taskResult.Match(
                    Succ: f,
                    Fail: Result<TResult>.Failure
                );
            }
            catch (Exception ex)
            {
                return Result<TResult>.Failure(ex);
            }
        }

        public static Task<Result<IEnumerable<TResult>>> ThenTraverseA<T, TResult>(this Task<Result<IEnumerable<T>>> taskResult, Func<T, Task<Result<TResult>>> f) =>
            taskResult.BindTaskResult(items => items.TraverseTaskResultA(f));

            
        public static Task<Result<IEnumerable<TResult>>> ThenTraverseASequentially<T, TResult>(this Task<Result<IEnumerable<T>>> taskResult, Func<T, Task<Result<TResult>>> f) =>
            taskResult.BindTaskResult(items => items.TraverseTaskResultASequentially(f));

        public static Task<Result<IEnumerable<TResult>>> ThenTraverseM<T, TResult>(this Task<Result<IEnumerable<T>>> taskResult, Func<T, Task<Result<TResult>>> f) =>
            taskResult.BindTaskResult(items => items.TraverseTaskResultM(f));
    }
}