using System;
using System.Linq;

namespace func {
    public static class ResultExtensions {
        public static Result<B> Map<A, B>(this Result<A> a, Func<A, B> f) {
            if (a.IsSuccess) {
                return Result<B>.Success(f(a.Value));
            }
            return Result<B>.Failure(a.Errors);
        }

        public static Result<B> Apply<A, B>(this Result<A> a, Result<Func<A, B>> f) {
            if (f.IsSuccess && a.IsSuccess)
                return Result<B>.Success(f.Value(a.Value));
            else
                return Result<B>.Failure(f.Errors.Concat(a.Errors).ToList());
        }
        
        public static Result<A> AsResult<A>(this A obj) {
            return Result<A>.Success(obj);
        }

        public static Result<B> Bind<A,B>(this Result<A> a, Func<A, Result<B>> f) {
            if (a.IsSuccess) {
                return f(a.Value);
            }

            return Result<B>.Failure(a.Errors);
        }
    }
}