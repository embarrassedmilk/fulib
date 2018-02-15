using System;
using System.Collections.Generic;
using System.Linq;

namespace Fulib
{
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
            return result.MatchTee(
                Succ: f,
                Fail: _ => { }
            );
        }

        public static Result<TResult> Bind<T,TResult>(this Result<T> a, Func<T, Result<TResult>> func) {
            return a.Match(
                Succ: func,
                Fail: errs => Result<TResult>.Failure(errs)
            );
        }

        public static Reader<E, Result<R>> EitherWithReader<E, T, R>(this Result<T> result, Func<T, Reader<E, Result<R>>> onSuccess, Func<IReadOnlyCollection<Error>, Reader<E, Result<R>>> onFailure)
            => api
            =>
            {
                Reader<E, Result<R>> reader = null;
                result.MatchTee(
                    Succ: v =>
                    {
                        reader = onSuccess(v);
                    },
                    Fail: er =>
                    {
                        reader = onFailure(er);
                    }
                );
                return reader.Run(api);
            };
    }
}