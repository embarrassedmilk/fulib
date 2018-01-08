using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace func {
    public class Car {
        public string Brand { get; set; }
        public int Id { get; set; }
    }

    public class WeirdCarCreator {
        private static Result<string> CreateBrand(string brandName) {
            if (string.IsNullOrEmpty(brandName)) {
                return Result.Failure<string>(new [] { "Brand name cannot be empty." });
            }
            return Result<string>.Success(brandName);
        }

        private static Result<int> CreateCarId(int id) {
            if (id <= 0) {
                return Result.Failure<int>(new [] { "Id must be positive." });
            }

            return Result<int>.Success(id);
        }

        private static Result<int> CreateMysticId(int id) {
            if (id != 666) {
                return Result<int>.Failure<int>(new [] { "Id is not mystic enough." });
            }

            return Result<int>.Success(id);
        }

        private static Car CreateCar(int id, int mysticId, string brandName) {
            return new Car { Id = id, Brand = brandName };
        }

        public static Result<Car> CreateCarA(int id, int mysticId, string brandName) {
            Func<int, int, string, Car> func = CreateCar;
            var curriedCreateCar = func.Curry().Flip();
 
            var applyResult = CreateCarId(id)
                                .Apply(CreateMysticId(mysticId)
                                    .Apply(CreateBrand(brandName)
                                        .Map(curriedCreateCar)));

            return applyResult;
        }
    }
}