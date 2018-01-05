using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace func {
    public static class ResultExtensions {
        public static Result<B> Map<A, B>(this Result<A> a, Func<A, B> f) {
            if (a.IsSuccess) {
                return new Result<B>(f(a.Value));
            }
            return new Result<B>(a.Errors);
        }

        public static Result<B> Apply<A, B>(this Result<A> a, Result<Func<A, B>> f) {
            if (f.IsSuccess && a.IsSuccess)
                return new Result<B>(f.Value(a.Value));
            else
                return new Result<B>(f.Errors.Concat(a.Errors).ToList());
        }
    }

    public static class FuncExtensions {
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> function) =>
            a => b => function(a, b);

        public static Func<T2, Func<T1, TResult>> Flip<T1, T2, TResult>(this Func<T1, Func<T2, TResult>> f) => 
            b => a => f(a)(b);
    }

    public class Result<A> {
        public Result(A val) {
            Value = val;
            Errors = new List<string>();
        }

        public Result(IReadOnlyCollection<string> errors) {
            Errors = errors;
        }

        public A Value { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public bool IsSuccess => !Errors.Any();
    }

    public class Car {
        public string Brand { get; set; }
        public int Id { get; set; }
    }

    public class WeirdCarCreator {
        private static Result<string> CreateBrand(string brandName) {
            if (string.IsNullOrEmpty(brandName)) {
                return new Result<string>(new [] { "Brand name cannot be empty." });
            }
            return new Result<string>(brandName);
        }

        private static Result<int> CreateCarId(int id) {
            if (id <= 0) {
                return new Result<int>(new [] { "Id must be positive." });
            }

            return new Result<int>(id);
        }

        private static Car CreateCar(int id, string brandName) {
            return new Car { Id = id, Brand = brandName };
        }

        public static Result<Car> CreateCarA(int id, string brandName) {
            Func<int, string, Car> func = CreateCar;
            var curriedCreateCar = func.Curry().Flip();
 
            var applyResult = CreateCarId(id)
                                .Apply(CreateBrand(brandName)
                                    .Map(curriedCreateCar));

            return applyResult;
        }
    }
}