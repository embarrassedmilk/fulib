using System.Collections.Generic;
using System.Linq;

namespace func {
    public class Result<T> {
        private Result(T val) {
            Value = val;
            Errors = new List<string>();
        }

        private Result(IReadOnlyCollection<string> errors) {
            Errors = errors;
        }

        public T Value { get; }

        public IReadOnlyCollection<string> Errors { get; protected set; }

        public bool IsSuccess => !Errors.Any();

        public static Result<T> Success(T val) {
            return new Result<T>(val);
        }

        public static Result<T> Failure(IReadOnlyCollection<string> errors) {
            return new Result<T>(errors);
        }
    }
}