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

        public static Result<List<B>> TraverseResultAppl<A,B>(this List<A> list, Func<A, Result<B>> f) {
            var initialState = Result.Success(new List<B>());

            Func<Result<List<B>>, A, Result<List<B>>> folder = (state, item) => {
                Func<B,List<B>,List<B>> prependFunc = Prepend;
                var cons = Result.Success(prependFunc.Curry().Flip());
                
                return f(item).Apply(state.Apply(cons));
            };

            return List.foldBack<Result<List<B>>, A>(list, initialState, folder);
        }

        public static Result<List<A>> SequenceResultAppl<A>(this List<Result<A>> list) {
            Func<Result<A>, Result<A>> id = (r) => new Identity<Result<A>>(r).Value;
            return list.TraverseResultAppl(id);
        }

        public static Task<List<B>> TraverseTaskAppl<A,B>(this List<A> list, Func<A, Task<B>> f) {
            var initialState = TaskExtensions.AsTask(new List<B>()); //AsTask - from l-ext

            Func<Task<List<B>>, A, Task<List<B>>> folder = (state, item) => {
                Func<B,List<B>,List<B>> prependFunc = Prepend;
                var cons = TaskExtensions.AsTask(prependFunc.Curry().Flip()); //AsTask - from l-ext
                
                return f(item).Apply(state.Apply(cons));
            };

            return List.foldBack<Task<List<B>>, A>(list, initialState, folder);
        }

        public static Task<List<A>> SequenceTaskAppl<A,B>(List<Task<A>> list) {
            Func<Task<A>, Task<A>> id = (t) => new Identity<Task<A>>(t).Value;
            return list.TraverseTaskAppl(id);
        }
    }
}