using System;

namespace func {
    public static class ReaderExtensions {
        public static T Run<E,T>(this Reader<E,T> action, E env)
            => action(env);

        public static Reader<E,R> Map<E,T,R>(this Reader<E,T> action, Func<T,R> f)
            => api
            => f(action.Run(api));

        public static Reader<E,R> Bind<E,T,R>(this Reader<E,T> action, Func<T,Reader<E,R>> f)
            => api
            => f(action.Run(api)).Run(api);

        public static Reader<E,T> AsReader<E,T>(this T obj)
            => api
            => obj;

        public static Reader<E,R> Apply<E,T,R>(this Reader<E,Func<T,R>> f, Reader<E,T> action)
            => api
            => f.Run(api)(action.Run(api));
    }
}