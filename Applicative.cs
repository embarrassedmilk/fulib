using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace func {
    public class Result<A> {
        public Result(A val)
        {
            Value = val;
            Errors = new List<string>();
        }

        public Result(IReadOnlyCollection<string> errors)
        {
            Errors = errors;
        }

        public A Value { get; }
        public IReadOnlyCollection<string> Errors { get; }

        public bool IsSuccess => !Errors.Any();

        public Result<B> Map<B>(Func<A,B> f, Result<A> a) {
            if (a.IsSuccess) {
                return new Result<B>(f(a.Value));
            }
            return new Result<B>(a.Errors);
        }

        public Result<B> Apply<B>(Result<Func<A,B>> f, Result<A> a) {
            if (f.IsSuccess && a.IsSuccess)
                return new Result<B>(f.Value(a.Value));
            else
                return new Result<B>(f.Errors.Concat(a.Errors).ToList());
        }
    }

    public class Car {
        public string Brand { get;set; }
        public int Id { get;set; }
    }

    public class WeirdCarCreator {
        private static Result<string> CreateBrand(string brandName) {
            if (string.IsNullOrEmpty(brandName)) {
                new Result<string>(new [] { "Brand name cannot be empty" });
            }
            return new Result<string>(brandName);
        }

        private static Result<int> CreateCarId(int id) {
            if (id <= 0) {
                return new Result<int>(new [] { "Id must be positive" });
            }

            return new Result<int>(id);
        }

        private static Car CreateCar(int id, string brandName) {
            return new Car { Id = id, Brand = brandName };
        }

        public static Result<Car> CreateCarA(int id, string brandName) {
            //need to curry CreateCar...
        }
    }
}