using System.Collections.Generic;
using System.Linq;

namespace func {
    public abstract class Result {
        protected Result() {}

        protected Result(IReadOnlyCollection<string> errors) {
            Errors = errors;
        }
        
        public IReadOnlyCollection<string> Errors { get; protected set; }

        public bool IsSuccess => !Errors.Any();

        public static Result<T> Success<T>(T val) {
            return new Result<T>(val);
        }

        public static Result<T> Failure<T>(IReadOnlyCollection<string> errors) {
            return new Result<T>(errors);
        }
    }

    public class Result<A> : Result {
        internal Result(A val) {
            Value = val;
            Errors = new List<string>();
        }

        internal Result(IReadOnlyCollection<string> errors): base(errors) { }

        public A Value { get; }
    }
}