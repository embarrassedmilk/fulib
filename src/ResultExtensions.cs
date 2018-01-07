using System;
using System.Linq;

namespace func {
    public static class ResultExtensions {
        public static Result<B> Map<A, B>(this Result<A> a, Func<A, B> f) {
            if (a.IsSuccess) {
                return new Result<B>(f(a.Value));
            }
            return new Result<B>(a.Errors);
        }

        public static Result<B> Apply<A, B>(this Result<A> a, Result<Func<A, B>> f) {
            if (f.IsSuccess && a.IsSuccess)
                return new Result<B>(f.Value(a.Value));
            else
                return new Result<B>(f.Errors.Concat(a.Errors).ToList());
        }
    }
}