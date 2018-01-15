using System;
using System.Threading.Tasks;

namespace func {
    public static class AsyncExtensions {
        public static Async<B> Map<A,B>(this Async<A> a, Func<A,B> f) {
            return new Async<B>(async () => {
                var aResult = await a.Run();
                
                return f(aResult);
            });
        }

        public static Async<B> Apply<A,B>(this Async<A> a, Async<Func<A,B>> f) {
            return new Async<B>(async () => {
                var aResult = await (a.Run());
                var fResult = await (f.Run());

                return fResult(aResult);
            });
        }

        public static Async<A> AsAsync<A>(this A a) {
            return new Async<A>(() => {
                return Task.FromResult(a);
            });
        }

        public static Async<B> Bind<A,B>(this Async<A> a, Func<A,Async<B>> f) {
            return new Async<B>(async () => {
                var aResult = await (a.Run());
                var fResult = await (f(aResult).Run());

                return fResult;
            });
        }
    }
}