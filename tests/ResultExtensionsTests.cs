using System;
using System.Linq;
using Xunit;
using func;
using FluentAssertions;

namespace tests
{
    public class ResultExtensionsTests 
    {
        [Fact]
        public void Apply_WithGivenFunctionAndArgument_AppliesArgToFunc()
        {
            var firstArg = 10;
            var secondArg = 2;
            var expectedResult = 12;
            Func<int, Func<int, int>> Sum = a => b => a + b;
            var elevatedSum = Sum.AsResult();

            var elevatedArg1 = firstArg.AsResult();
            var elevatedArg2 = secondArg.AsResult();

            var result = elevatedSum.Apply(elevatedArg1).Apply(elevatedArg2);

            result.Match(
                Succ: v => {
                    v.Should().Be(expectedResult);
                    return v.AsResult();
                },
                Fail: _ => throw new Exception("bla")
            );
        }

        [Fact]
        public void Apply_WithGivenFunctionAndFailedArguments_ReturnsAggregatedErrors()
        {
            const string FIRST_ERROR = "first error";
            const string SECOND_ERROR = "second error";
            var expectedErrors = new[] {FIRST_ERROR, SECOND_ERROR};
            Func<int, Func<int, int>> Sum = a => b => a + b;
            var elevatedSum = Sum.AsResult();

            var elevatedArg1 = Result<int>.Failure(FIRST_ERROR);
            var elevatedArg2 = Result<int>.Failure(SECOND_ERROR);

            var result = elevatedSum.Apply(elevatedArg1).Apply(elevatedArg2);

            result.Match(
                Succ: _ => throw new Exception("bla"),
                Fail: errs => {
                    errs.Select(x=>x.Message).ShouldBeEquivalentTo(expectedErrors);
                    return Result<int>.Failure(errs);
                }
            );
        }

        [Fact]
        public void Apply_WithFaultedArgs_DoesNotCallFuncToApply() {
            int numberOfInvocations = 0;
            
            Func<int, Func<int, int>> Sum = a => {
                numberOfInvocations++;
                return b => 
                {
                    numberOfInvocations++;
                    return a + b;
                };
            };

            var elevatedSum = Sum.AsResult();

            var elevatedArg1 = Result<int>.Failure(string.Empty);
            var elevatedArg2 = Result<int>.Failure(string.Empty);

            elevatedSum.Apply(elevatedArg1).Apply(elevatedArg2);

            numberOfInvocations.Should().Be(0);
        }

        [Fact]
        public void Apply_WithFaultedFunc_ReturnsErrorsFromFunc() {
            const string FUNC_ERROR = "func is faulted";
            var expectedErrors = new [] { FUNC_ERROR };          
            var elevatedFaultedFunc = Result<Func<int, Func<int, int>>>.Failure(FUNC_ERROR);

            var elevatedArg1 = 2.AsResult();
            var elevatedArg2 = 3.AsResult();

            var result = elevatedFaultedFunc.Apply(elevatedArg1).Apply(elevatedArg2);

            result.Match(
                Succ: _ => throw new Exception("bla"),
                Fail: errs => {
                    errs.Select(x=>x.Message).ShouldBeEquivalentTo(expectedErrors);
                    return Result<int>.Failure(errs);
                }
            );
        }

        [Fact]
        public void Apply_WithFaultedFuncAndFaultedArgs_ReturnsAllErrors() {
            const string FUNC_ERROR = "func is faulted";
            const string FIRST_ARG_ERROR = "first error";
            const string SECOND_ARG_ERROR = "second error";

            var expectedErrors = new [] { FUNC_ERROR, FIRST_ARG_ERROR, SECOND_ARG_ERROR };          
            
            var elevatedFaultedFunc = Result<Func<int, Func<int, int>>>.Failure(FUNC_ERROR);
            var elevatedArg1 = Result<int>.Failure(FIRST_ARG_ERROR);
            var elevatedArg2 = Result<int>.Failure(SECOND_ARG_ERROR);

            var result = elevatedFaultedFunc.Apply(elevatedArg1).Apply(elevatedArg2);

            result.Match(
                Succ: _ => throw new Exception("bla"),
                Fail: errs => {
                    errs.Select(x=>x.Message).ShouldBeEquivalentTo(expectedErrors);
                    return Result<int>.Failure(errs);
                }
            );
        }
    }
}