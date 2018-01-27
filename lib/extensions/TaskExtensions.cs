using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class TaskAsyncExtensions {
        public async static Task<B> Apply<A,B>(this Task<A> task, Task<Func<A,B>> f) {
            var taskResult = await task;
            var fResult = await f;
            return fResult(taskResult);
        }
    }
}