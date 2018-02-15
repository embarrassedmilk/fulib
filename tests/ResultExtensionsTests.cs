using System;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace Fulib.Tests
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

            result.ExtractValueUnsafe().Should().Be(expectedResult);
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

            result.ExtractErrorsUnsafe().Select(x => x.Message).ShouldBeEquivalentTo(expectedErrors);
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

            result.ExtractErrorsUnsafe().Select(x => x.Message).ShouldBeEquivalentTo(expectedErrors);
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

            result.ExtractErrorsUnsafe().Select(x => x.Message).ShouldBeEquivalentTo(expectedErrors);
        }

        [Fact]
        public void AsResult_WrapsValue()
        {
            var value = new object();

            value.AsResult().ExtractValueUnsafe().Should().Be(value);
        }

        [Fact]
        public void Bind_OnSuccessfulResult_InvokesBind()
        {
            var bindInvoked = false;
            var sut = Unit.Default.AsResult();

            Result<Unit> Bind(Unit unit)
            {
                bindInvoked = true;
                return sut;
            }

            sut.Bind(Bind);

            bindInvoked.Should().BeTrue();
        }

        [Fact]
        public void Bind_OnFailedResult_DoesNotCallBind()
        {
            var bindInvoked = false;
            var sut = Result<Unit>.Failure(string.Empty);

            Result<Unit> Bind(Unit val)
            {
                bindInvoked = true;
                return sut;
            }

            sut.Bind(Bind);

            bindInvoked.Should().BeFalse();
        }

        [Fact]
        public void Map_OnSuccessfulResult_CallsMap()
        {
            var mapInvoked = false;
            var sut = Unit.Default.AsResult();

            Unit Map(Unit unit)
            {
                mapInvoked = true;
                return Unit.Default;
            }

            sut.Map(Map);

            mapInvoked.Should().BeTrue();
        }

        [Fact]
        public void Map_OnFailedResult_DoesNotCallMap()
        {
            var mapInvoked = false;
            var sut = Result<Unit>.Failure(string.Empty);

            Unit Map(Unit val)
            {
                mapInvoked = true;
                return Unit.Default;
            }

            sut.Map(Map);

            mapInvoked.Should().BeFalse();
        }

        [Fact]
        public void Tee_OnFailedResult_DoesNotCallTee()
        {
            var teeInvoked = false;
            var sut = Result<Unit>.Failure(string.Empty);

            void Tee(Unit unit)
            {
                teeInvoked = true;
            }

            sut.Tee(Tee);

            teeInvoked.Should().BeFalse();
        }

        [Fact]
        public void Tee_OnSuccessfulResult_CallsTee()
        {
            var teeInvoked = false;
            var sut = Unit.Default.AsResult();

            void Tee(Unit unit)
            {
                teeInvoked = true;
            }

            sut.Tee(Tee);

            teeInvoked.Should().BeTrue();
        }
    }
}