using System;

namespace func {
    public static class MiddlwareExtensions {
        public static Middleware<R> Bind<T,R>(this Middleware<T> middleware, Func<T,Middleware<R>> f)
            => cont 
            => middleware(t => f(t)(cont));
    }
}