using System;

namespace func {
    public static class FuncExtensions {
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> function) =>
            t1 => t2 => function(t1, t2);

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> function) =>
            t1 => t2 => t3 => function(t1, t2, t3);

        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> function) =>
            t1 => t2 => t3 => t4 => function(t1, t2, t3, t4);

        public static Func<T2, Func<T1, TResult>> Flip<T1, T2, TResult>(this Func<T1, Func<T2, TResult>> f) => 
            t2 => t1 => f(t1)(t2);

        public static Func<T3, Func<T2, Func<T1, TResult>>> Flip<T1, T2, T3, TResult>(this Func<T1, Func<T2, Func<T3, TResult>>> f) => 
            t3 => t2 => t1 => f(t1)(t2)(t3);

        public static Func<T4, Func<T3, Func<T2, Func<T1, TResult>>>> Flip<T1, T2, T3, T4, TResult>(this Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> f) => 
            t4 => t3 => t2 => t1 => f(t1)(t2)(t3)(t4);
    }
}