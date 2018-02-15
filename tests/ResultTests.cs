using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Fulib.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Match_OnSucccessfulResult_CallsSuccessFunc()
        {
            var successInvoked = false;

            var result = Unit.Default.AsResult();

            result.MatchTee(
                val => successInvoked = true,
                _ => { }
            );

            successInvoked.Should().BeTrue();
        }

        [Fact]
        public void Match_OnFailedResult_CallsFailureFunc()
        {
            const string ERROR_TEXT = "error";
            var failInvoked = false;
            var result = Result<Unit>.Failure(ERROR_TEXT);

            result.MatchTee(
                _ => { },
                err => failInvoked = true
            );

            failInvoked.Should().BeTrue();
        }

        [Fact]
        public void MatchTee_OnSucccessfulResult_CallsSuccessFunc()
        {
            var successInvoked = false;

            var result = Unit.Default.AsResult();

            result.MatchTee(
                val => successInvoked = true,
                _ => { }
            );

            successInvoked.Should().BeTrue();
        }

        [Fact]
        public void MatchTee_OnFailedResult_CallsFailureFunc()
        {
            const string ERROR_TEXT = "error";
            var failInvoked = false;
            var result = Result<Unit>.Failure(ERROR_TEXT);

            result.MatchTee(
                _ => { },
                err => failInvoked = true
            );

            failInvoked.Should().BeTrue();
        }


        [Fact]
        public void MatchTee_ReturnsOriginalValue()
        {
            var sut = Unit.Default.AsResult();

            var result = sut.MatchTee(
                val => { },
                _ => { }
            );

            result.Should().Be(sut);
        }

        [Fact]
        public async Task MatchTeeAsync_ReturnsOriginalValue()
        {
            var sut = Unit.Default.AsResult();

            var result = await sut.MatchTeeAsync(
                _ => Task.CompletedTask,
                _ => { }
            );

            result.Should().Be(sut);
        }

        [Fact]
        public async Task MatchAsync_OnSuccessfulResult_CallsSuccessFunc()
        {
            var successInvoked = false;

            var result = Unit.Default.AsResult();

            await result.MatchTeeAsync(
                _ =>
                {
                    successInvoked = true;
                    return Task.CompletedTask;
                },
                _ => { }
            );

            successInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_OnFailedResult_CallsFailureFunc()
        {
            const string ERROR_TEXT = "error";
            var failureInvoked = false;
            var result = Result<Unit>.Failure(ERROR_TEXT);

            await result.MatchTeeAsync(
                _ => Task.CompletedTask,
                _ => failureInvoked = true
            );

            failureInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task MatchTeeAsync_OnFailedResult_CallsFailureFunc()
        {
            const string ERROR_TEXT = "error";
            var failureInvoked = false;
            var result = Result<Unit>.Failure(ERROR_TEXT);

            await result.MatchTeeAsync(
                _ => Task.CompletedTask,
                err =>
                {
                    failureInvoked = true;
                }
            );

            failureInvoked.Should().BeTrue();
        }

        [Fact]
        public async Task MatchTeeAsync_OnSuccessfulResult_CallsSuccessFunc()
        {
            var successInvoked = false;
            var result = Unit.Default.AsResult();
            await result.MatchTeeAsync(
                _ =>
                {
                    successInvoked = true;
                    return Task.CompletedTask;
                },
                _ => { }
            );

            successInvoked.Should().BeTrue();
        }
    }
}
