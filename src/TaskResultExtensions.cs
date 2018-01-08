using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class TaskResultExtensions {
        public static Task<Result<B>> MapL<A,B>(this Task<Result<A>> task, Func<A,B> f) => task.Map(r => r.Map(f));
    }
}