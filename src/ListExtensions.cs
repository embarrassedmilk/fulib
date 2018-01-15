using LanguageExt;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace func {
    public static class ListResultExtensions {
        private static List<T> Prepend<T>(T item, List<T> list) {
            list.Insert(0, item);

            return list;
        }

        public static Result<List<B>> TraverseResultA<A,B>(this IEnumerable<A> list, Func<A, Result<B>> f) {
            var initialState = new List<B>().AsResult();

            Func<Result<List<B>>, A, Result<List<B>>> folder = (state, item) => {
                Func<B,List<B>,List<B>> prependFunc = Prepend;
                var cons = prependFunc.Curry().Flip().AsResult();
                return f(item).Apply(state.Apply(cons));
            };

            return List.foldBack<Result<List<B>>, A>(list, initialState, folder);
        }

        public static Result<List<A>> SequenceResultA<A>(this IEnumerable<Result<A>> list) {
            Func<Result<A>, Result<A>> id = (r) => new Identity<Result<A>>(r).Value;
            return list.TraverseResultA(id);
        }

        public static Task<List<B>> TraverseTaskA<A,B>(this IEnumerable<A> list, Func<A, Task<B>> f) {
            var initialState = new List<B>().AsTask();

            Func<Task<List<B>>, A, Task<List<B>>> folder = (state, item) => {
                Func<B,List<B>,List<B>> prependFunc = Prepend;
                var cons = prependFunc.Curry().Flip().AsTask();
                
                return f(item).Apply(state.Apply(cons));
            };

            return List.foldBack<Task<List<B>>, A>(list, initialState, folder);
        }

        public static Task<List<A>> SequenceTaskA<A>(this IEnumerable<Task<A>> list) {
            Func<Task<A>, Task<A>> id = (t) => new Identity<Task<A>>(t).Value;
            return list.TraverseTaskA(id);
        }

        public static Task<Result<List<B>>> TraverseTaskResultM<A,B>(this IEnumerable<A> list, Func<A, Task<Result<B>>> f) {
            var initialState = new List<B>().AsResult().AsTask();

            Func<Task<Result<List<B>>>, A, Task<Result<List<B>>>> folder = (state, item) => 
                f(item).BindLocal(h => state.BindLocal(t => Prepend(h,t).AsResult().AsTask()));
    
            return list.FoldBack<Task<Result<List<B>>>, A>(initialState, folder);
        }

        public static Task<Result<List<A>>> SequenceTaskResultM<A>(this IEnumerable<Task<Result<A>>> list) {
            Func<Task<Result<A>>, Task<Result<A>>> id = (t) => new Identity<Task<Result<A>>>(t).Value;
            return list.TraverseTaskResultM(id);
        }

        public static Async<Result<List<B>>> TraverseAsyncResultM<A,B>(this IEnumerable<A> list, Func<A, Async<Result<B>>> f) {
            var initialState = new List<B>().AsResult().AsAsync();

            Func<Async<Result<List<B>>>, A, Async<Result<List<B>>>> folder = (state, item) => 
                f(item).BindAR(h => state.BindAR(t => Prepend(h,t).AsResult().AsAsync()));
    
            return list.FoldBack<Async<Result<List<B>>>, A>(initialState, folder);
        }

        public static Async<Result<List<A>>> SequenceAsyncResult<A>(this IEnumerable<Async<Result<A>>> list) {
            Func<Async<Result<A>>, Async<Result<A>>> id = (t) => new Identity<Async<Result<A>>>(t).Value;
            return list.TraverseAsyncResultM(id);
        }
    }
}