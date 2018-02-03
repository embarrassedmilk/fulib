using System;

namespace func {
    public delegate T ApiAction<T>(ApiClient api);

    public static class ApiActionExtensions {
        public static T Run<T>(this ApiAction<T> action, ApiClient client)
            => action(client);

        public static ApiAction<R> Map<T,R>(this ApiAction<T> action, Func<T,R> f)
            => api
            => f(action.Run(api));

        public static ApiAction<R> Bind<T,R>(this ApiAction<T> action, Func<T,ApiAction<R>> f)
            => api
            => f(action.Run(api)).Run(api);

        public static ApiAction<T> AsApiAction<T>(this T obj)
            => api
            => obj;

        public static ApiAction<R> Apply<T,R>(this ApiAction<Func<T,R>> f, ApiAction<T> action)
            => api
            => f.Run(api)(action.Run(api));

        public static T Execute<T>(this ApiAction<T> action) {
            using (var client = new ApiClient()) {
                return action(client);
            }
        }
    }
}