using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public static class ResultExtensions {
        public static Result<TResult> Map<T, TResult>(this Result<T> a, Func<T, TResult> func) {
            return a.Match(
                Succ: val => func(val).AsResult(),
                Fail: errs => Result<TResult>.Failure(errs)
            );
        }
        
        public static Result<TResult> Apply<T, TResult>(this Result<Func<T, TResult>> f, Result<T> result) {
            return result.Match(
                Succ: val => f.Match(
                    Succ: func => func(val).AsResult(),
                    Fail: Result<TResult>.Failure
                ),
                Fail: resultErrs => f.Match(
                    Succ: _ => Result<TResult>.Failure(resultErrs),
                    Fail: funcErrs => Result<TResult>.Failure(funcErrs.Concat(resultErrs).ToList())
                )
            );
        }
        
        public static Result<T> AsResult<T>(this T obj) {
            return Result<T>.Success(obj);
        }

        public static Result<T> Tee<T>(this Result<T> result, Action<T> f) {
            return result.Match(
                Succ: val => {
                    f(val);
                    return val.AsResult();
                },
                Fail: Result<T>.Failure
            );
        }

        public static Result<T> TryCatchTee<T>(this Result<T> result, Action<T> f) {
            try {
                return result.Tee(f);
            }
            catch (Exception ex) {
                return Result<T>.Failure(ex);
            }
        }

        public static Result<TResult> Bind<T,TResult>(this Result<T> a, Func<T, Result<TResult>> func) {
            return a.Match(
                Succ: func,
                Fail: errs => Result<TResult>.Failure(errs)
            );
        }
    }
}