using System;
using System.Collections.Generic;
using System.Linq;

namespace func {
    public class Car {
        public string Brand { get; set; }
        public int Id { get; set; }
    }

    public class ApplyExample {
        private static Result<string> CreateBrand(string brandName) {
            if (string.IsNullOrEmpty(brandName)) {
                return Result<string>.Failure("Brand name cannot be empty.");
            }
            return Result<string>.Success(brandName);
        }

        private static Result<int> CreateCarId(int id) {
            if (id <= 0) {
                return Result<int>.Failure("Id must be positive.");
            }

            return Result<int>.Success(id);
        }

        private static Result<int> CreateMysticId(int id) {
            if (id != 666) {
                return Result<int>.Failure("Id is not mystic enough.");
            }

            return Result<int>.Success(id);
        }

        private static Car CreateCar(int id, int mysticId, string brandName) {
            return new Car { Id = id, Brand = brandName };
        }

        public static Result<Car> CreateCarA(int id, int mysticId, string brandName) {
            Func<int, int, string, Car> createFunc = CreateCar;
            var curriedCreateCar = createFunc.Curry().AsResult();

            return curriedCreateCar
                        .Apply(CreateCarId(id))
                        .Apply(CreateMysticId(mysticId))
                        .Apply(CreateBrand(brandName));
        }
    }
}