using System;
using System.Threading.Tasks;

namespace func {
    public class Async<T> {
        private readonly Func<Task<T>> _func;

        public Async(Func<Task<T>> f) {
            _func = f;
        }

        public Task<T> Run() {
            return _func();
        }
    }
}