using System;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

namespace Fulib.Tests
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
            taskResult.ExtractValueUnsafe().Should().Be(expectedResult);
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

            result.ExtractErrorsUnsafe().Select(x => x.Message).ShouldBeEquivalentTo(expectedErrors);
        }

        [Fact]
        public async Task BindTaskResult_WithSuccessfulTask_CallsBind()
        {
            var bindInvoked = false;
            var sut = Unit.Default.AsTaskResult();

            Task<Result<Unit>> BindingFunc(Unit unit)
            {
                bindInvoked = true;
                return sut;
            }

            await sut.BindTaskResult(BindingFunc);

            bindInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task BindTaskResult_WithFaultedTask_DoesNotCallBind()
        {
            var bindInvoked = false;
            var faultedTask = GetFaultedTask();

            Task<Result<Unit>> Bind(Unit unit)
            {
                bindInvoked = true;
                return Unit.Default.AsTaskResult();
            }

            await faultedTask.BindTaskResult(Bind);

            bindInvoked.Should().BeFalse();
        }

        [Fact]
        public async Task MapTaskResult_WithSuccessfulTask_CallsMap()
        {
            var mapInvoked = false;
            var wrappedValue = Unit.Default.AsTaskResult();

            Unit MappingFunc(Unit unit)
            {
                mapInvoked = true;
                return unit;
            }

            await wrappedValue.MapTaskResult(MappingFunc);

            mapInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task MapTaskResult_WithFaultedTask_DoesNotCallMap()
        {
            var mapInvoked = false;
            var wrappedValue = GetFaultedTask();

            Unit Map(Unit unit)
            {
                mapInvoked = true;
                return unit;
            }

            await wrappedValue.MapTaskResult(Map);

            mapInvoked.Should().BeFalse();
        }

        [Fact]
        public async Task Then_WithSuccessfulTask_CallsNextFunc()
        {
            var funcInvoked = false;
            var sut = Unit.Default.AsTaskResult();

            Result<Unit> NextFunc(Unit unit)
            {
                funcInvoked = true;
                return unit.AsResult();
            }

            await sut.Then(NextFunc);

            funcInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task Then_WithFaultedTask_DoesNotCallNextFunc()
        {
            var funcInvoked = false;
            var faultedTask = GetFaultedTask();

            Result<Unit> NextFunc(Unit unit)
            {
                funcInvoked = true;
                return unit.AsResult();
            }

            await faultedTask.Then(NextFunc);

            funcInvoked.Should().BeFalse();
        }

        private Task<Result<Unit>> GetFaultedTask()
        {
            return Task.FromException<Result<Unit>>(new Exception());
        }
    }
}