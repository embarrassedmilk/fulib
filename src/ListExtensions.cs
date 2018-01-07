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
            var initialState = new Result<List<B>>(new List<B>());

            Func<Result<List<B>>, A, Result<List<B>>> folder = (state, item) => {
                //apply new result with f(item) with state...
                return null;
            };

            return List.foldBack<Result<List<B>>, A>(list, initialState, folder);
        }
    }
}