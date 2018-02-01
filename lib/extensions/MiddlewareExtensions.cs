using System;

namespace func {
    public static class MiddlwareExtensions {
        public static Middleware<R> Bind<T,R>(this Middleware<T> middleware, Func<T,Middleware<R>> f)
            => cont 
            => middleware(t => f(t)(cont));

        public static Middleware<R> Map<T,R>(this Middleware<T> middleware, Func<T,R> f)
            => cont 
            => middleware(t=> cont(f(t)));

        public static T Run<T>(this Middleware<T> middleware)
            => (T) middleware(t => t);
    }
}