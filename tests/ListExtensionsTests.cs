using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using func; 
using FluentAssertions;

using Unit = System.ValueTuple;

namespace tests
{
    public class ListExtensionsTests
    {
        [Fact]
        public async Task TraverseTaskResultM_WithAllSuccessfulResults_ReturnsSuccessfulResult()
        {
            var taskResult = Enumerable.Range(0, 10).TraverseTaskResultM(i => GetSuccessfulTaskResult());

            var result = await taskResult;

            result.Match(
                Succ: val =>  val.AsResult(),
                Fail: _ => throw new Exception("Should be success")
            );
        }

        [Fact]
        public async Task TraverseTaskResultM_WithAllSuccessfulResultsAndOneFailed_ReturnsFailedResult()
        {
            const string ERROR_TEXT = "error";
            
            var taskResult = Enumerable.Range(0, 10).TraverseTaskResultM(i =>
                i == 3
                ? GetFailedTaskResult(ERROR_TEXT) 
                : GetSuccessfulTaskResult());

            var result = await taskResult;

            result.Match(
                Succ: _ =>  throw new Exception("Should be failure"),
                Fail: errs => {
                    errs.Select(x=>x.Message).Should().Contain(ERROR_TEXT);
                    return Result<Unit>.Failure(errs);
                }
            );
        }

        [Fact]
        public async Task TraverseTaskResultM_WithAllSuccessfulResults_IteratesThroughAllElements()
        {
            var numberOfElements = 10;
            var numberOfInvocations = 0;
            var taskResult = Enumerable.Range(0, numberOfElements)
                .TraverseTaskResultM(i => {
                        numberOfInvocations++;
                        return GetSuccessfulTaskResult();
                    }
                );

            await taskResult;

            numberOfInvocations.Should().Be(numberOfElements);
        }

        [Fact]
        public async Task TraverseTaskResultM_WithAllSuccessfulResultsAndOneFailed_StopsAtFailure()
        {
            var numberOfElements = 10;
            var numberOfInvocations = 0;
            var failAt = 3;
            var taskResult = Enumerable.Range(1, numberOfElements)
                .TraverseTaskResultM(i => {
                        numberOfInvocations++;
                        return i == failAt ? GetFailedTaskResult(string.Empty) : GetSuccessfulTaskResult();
                    }
                );

            await taskResult;

            numberOfInvocations.Should().Be(failAt);
        }

        [Fact]
        public async Task TraverseTaskResultA_WithAllSuccessfulResults_ReturnsSuccessfulResult()
        {
            var taskResult = Enumerable.Range(0, 10).TraverseTaskResultA(i => GetSuccessfulTaskResult());

            var result = await taskResult;

            result.Match(
                Succ: val =>  val.AsResult(),
                Fail: _ => throw new Exception("Should be success")
            );
        }

        [Fact]
        public async Task TraverseTaskResultA_WithAllSuccessfulResultsAndOneFailed_ReturnsFailedResult()
        {
            const string ERROR_TEXT = "error";
            
            var taskResult = Enumerable.Range(0, 10).TraverseTaskResultA(i =>
                i == 3
                ? GetFailedTaskResult(ERROR_TEXT) 
                : GetSuccessfulTaskResult());

            var result = await taskResult;

            result.Match(
                Succ: _ =>  throw new Exception("Should be failure"),
                Fail: errs => {
                    errs.Select(x=>x.Message).Should().Contain(ERROR_TEXT);
                    return Result<Unit>.Failure(errs);
                }
            );
        }

        [Fact]
        public async Task TraverseTaskResultA_WithAllSuccessfulResults_IteratesThroughAllInputValues()
        {
            var numberOfElements = 10;
            var numberOfInvocations = 0;
            var taskResult = Enumerable.Range(0, numberOfElements)
                .TraverseTaskResultA(i => {
                        numberOfInvocations++;
                        return GetSuccessfulTaskResult();
                    }
                );

            await taskResult;

            numberOfInvocations.Should().Be(numberOfElements);
        }

        [Fact]
        public async Task TraverseTaskResultA_WithAllSuccessfulResultsAndOneFailed_IteratesThroughAllInputValues()
        {
            var numberOfElements = 10;
            var numberOfInvocations = 0;
            var failAt = 3;
            var taskResult = Enumerable.Range(1, numberOfElements)
                .TraverseTaskResultA(i => {
                        numberOfInvocations++;
                        return i == failAt ? GetFailedTaskResult(string.Empty) : GetSuccessfulTaskResult();
                    }
                );

            await taskResult;

            numberOfInvocations.Should().Be(numberOfElements);
        }

        [Fact]
        public async Task TraverseTaskResultA_WithAllSuccessfulResultsAndSomeFailed_CollectsAllErrors()
        {
            var errorText = "error";
            var expectedFailures = new [] { errorText, errorText };
            var failAt = 3;
            var thenFailAt = 5;
            
            var taskResult = Enumerable.Range(0, 10).TraverseTaskResultA(i =>
                (i == failAt || i == thenFailAt)
                ? GetFailedTaskResult(errorText) 
                : GetSuccessfulTaskResult());

            var result = await taskResult;

            result.Match(
                Succ: _ =>  throw new Exception("Should be failure"),
                Fail: errs => {
                    errs.Select(x=>x.Message).Should().BeEquivalentTo(expectedFailures);
                    return Result<Unit>.Failure(errs);
                }
            );
        }

        private Task<Result<Unit>> GetSuccessfulTaskResult()
        {
            return default(Unit).AsTaskResult();
        }

        private Task<Result<Unit>> GetFailedTaskResult(string error)
        {
            return Result<Unit>.Failure(error).AsTask();
        }
    }
}