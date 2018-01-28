using System;
using System.Linq;
using Xunit;
using func;
using System.Threading.Tasks;
using FluentAssertions;

namespace tests
{
    public class TaskResultExtensionsTests
    {
        [Fact] 
        public async Task ApplyTaskResult_WithGivenFunctionAndArgument_AppliesArgToFunc() 
        {
            var firstArg = 10;
            var secondArg = 2;
            var expectedResult = 12;
            Func<int, Func<int, int>> Sum = a => b => a + b;
            var elevatedSum = Sum.AsTaskResult();

            var elevatedArg1 = firstArg.AsTaskResult();
            var elevatedArg2 = secondArg.AsTaskResult();

            var taskResult = await elevatedSum.ApplyTaskResult(elevatedArg1).ApplyTaskResult(elevatedArg2);

            taskResult.Match(
                Succ: v => {
                    v.Should().Be(expectedResult);
                    return v.AsResult();
                },
                Fail: _ => throw new Exception("bla")
            );
        }

        [Fact]
        public async Task ApplyTaskResult_WithGivenFunctionAndFailedArguments_ReturnsAggregatedErrors()
        {
            const string FIRST_ERROR = "first error";
            const string SECOND_ERROR = "second error";
            var expectedErrors = new[] {FIRST_ERROR, SECOND_ERROR};
            Func<int, Func<int, int>> Sum = a => b => a + b;
            var elevatedSum = Sum.AsTaskResult();

            var elevatedArg1 = Result<int>.Failure(FIRST_ERROR).AsTask();
            var elevatedArg2 = Result<int>.Failure(SECOND_ERROR).AsTask();

            var result = await elevatedSum.ApplyTaskResult(elevatedArg1).ApplyTaskResult(elevatedArg2);

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