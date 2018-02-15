using System;

namespace Fulib {
    public static class LinqMiddlewareExtensions {
        public static Middleware<R> Select<T,R>(this Middleware<T> mw, Func<T,R> f)
            => mw.Map(f);

        public static Middleware<RR> SelectMany<T,R,RR>(this Middleware<T> mw, Func<T, Middleware<R>> bind, Func<T,R,RR> proj) 
            => mw.Bind(t => bind(t).Map(r => proj(t,r)));
    }
}