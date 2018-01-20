using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public static class ResultExtensions {
        public static Result<B> Match<A,B>(this Result<A> a, Func<A, Result<B>> Succ, Func<IReadOnlyCollection<string>, Result<B>> Fail) {
            if (!a.IsSuccess) {
                return Fail(a.Errors);
            }
            return Succ(a.Value);
        }

        public static Result<B> Map<A, B>(this Result<A> a, Func<A, B> func) {
            return a.Match(
                Succ: val => func(val).AsResult(),
                Fail: errs => Result<B>.Failure(errs)
            );
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

        public static Result<A> Tee<A>(this Result<A> result, Action<A> f) {
            return result.Match(
                Succ: val => {
                    f(val);
                    return val.AsResult();
                },
                Fail: Result<A>.Failure
            );
        }

        public static Result<A> TryCatchTee<A>(this Result<A> result, Action<A> f) {
            try {
                return result.Tee(f);
            }
            catch (Exception ex) {
                return Result<A>.Failure(new List<string> {ex.Message});
            }
        }

        public static Result<B> Bind<A,B>(this Result<A> a, Func<A, Result<B>> func) {
            return a.Match(
                Succ: func,
                Fail: errs => Result<B>.Failure(errs)
            );
        }
    }
}