using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace func {
    public class Result<T> {
        private Result(T val) {
            Value = val;
            Errors = new List<Error>();
        }

        private Result(IReadOnlyCollection<Error> errors) {
            Errors = errors;
        }

        private T Value { get; }

        private IReadOnlyCollection<Error> Errors { get; }

        private bool IsSuccess => !Errors.Any();

        public static Result<T> Success(T val) {
            return new Result<T>(val);
        }

        public static Result<T> Failure(string message) {
            return Failure(new [] { new Error(message) });
        }

        public static Result<T> Failure(Exception ex) {
            return Failure(new [] { new Error(ex) });
        }

        public static Result<T> Failure(IReadOnlyCollection<Error> errors) {
            return new Result<T>(errors);
        }

        public Result<TResult> Match<TResult>(Func<T, Result<TResult>> Succ, Func<IReadOnlyCollection<Error>, Result<TResult>> Fail) {
            if (!IsSuccess) {
                return Fail(Errors);
            }
            return Succ(Value);
        }
        
        public TResult Match<TResult>(Func<T, TResult> Succ, Func<IReadOnlyCollection<Error>, TResult> Fail) {
            if (!IsSuccess) {
                return Fail(Errors);
            }
            return Succ(Value);
        }

        public async Task<Result<TResult>> MatchAsync<TResult>(Func<T, Task<Result<TResult>>> Succ, Func<IReadOnlyCollection<Error>, Result<TResult>> Fail)
        {
            if (!IsSuccess)
                return Fail(Errors);

            return await Succ(Value);
        }
    }
}