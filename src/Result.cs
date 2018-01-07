using System.Collections.Generic;
using System.Linq;

namespace func {
    public class Result<A> {
        public Result(A val) {
            Value = val;
            Errors = new List<string>();
        }

        public Result(IReadOnlyCollection<string> errors) {
            Errors = errors;
        }

        public A Value { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public bool IsSuccess => !Errors.Any();
    }
}