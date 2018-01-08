using LanguageExt;
using System;
using System.Collections.Generic;

namespace func {
    public static class ListResultExtensions {
        private static List<T> Prepend<T>(T item, List<T> list) {
            list.Insert(0, item);

            return list;
        }

        public static Result<List<B>> TraverseResultAppl<A,B>(Func<A, Result<B>> f, List<A> list) {
            var initialState = Result.Success(new List<B>());

            Func<Result<List<B>>, A, Result<List<B>>> folder = (state, item) => {
                Func<B,List<B>,List<B>> prependFunc = Prepend;
                var cons = Result.Success(prependFunc.Curry().Flip());
                
                return f(item).Apply(state.Apply(cons));
            };

            return List.foldBack<Result<List<B>>, A>(list, initialState, folder);
        }

        public static Result<List<A>> SequenceResultAppl<A>(List<Result<A>> list) {
            Func<Result<A>, Result<A>> id = (r) => new Identity<Result<A>>(r).Value;
            return TraverseResultAppl(id, list);
        }

        
    }
}