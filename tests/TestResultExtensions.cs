using System;
using System.Collections.Generic;
using System.Linq;
namespace Fulib.Tests
{
    public static class TestResultExtensions
    {
        public static T ExtractValueUnsafe<T>(this Result<T> result)
        {
            bool isSuccess = false;
            IEnumerable<Error> errors = Enumerable.Empty<Error>();
            T val = default(T);

            result.MatchTee
            (
                Succ: v => 
                {
                    val = v;
                    isSuccess = true;
                },
                Fail: err => errors = err
            );

            if (!isSuccess)
            {
                throw new Exception($"Expected successful result, got errors instead {errors}");
            }

            return val;
        }


        public static IEnumerable<Error> ExtractErrorsUnsafe<T>(this Result<T> result)
        {
            bool isSuccess = false;
            IEnumerable<Error> errors = Enumerable.Empty<Error>();
            T val = default(T);

            result.MatchTee
            (
                Succ: v =>
                {
                    val = v;
                    isSuccess = true;
                },
                Fail: err => errors = err
            );

            if (isSuccess)
            {
                throw new Exception($"Expected failed result, got value instead {val}");
            }

            return errors;
        }
    }
}
