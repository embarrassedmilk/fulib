using System;
using System.Linq;

namespace func {
    public static class ResultExtensions {
        public static Result<B> Map<A, B>(this Result<A> a, Func<A, B> f) {
            if (a.IsSuccess) {
                return Result.Success(f(a.Value));
            }
            return Result.Failure<B>(a.Errors);
        }

        public static Result<B> Apply<A, B>(this Result<A> a, Result<Func<A, B>> f) {
            if (f.IsSuccess && a.IsSuccess)
                return Result.Success(f.Value(a.Value));
            else
                return Result.Failure<B>(f.Errors.Concat(a.Errors).ToList());
        }
    }
}