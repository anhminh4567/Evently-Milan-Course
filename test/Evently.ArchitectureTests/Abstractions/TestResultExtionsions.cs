using FluentAssertions;
using NetArchTest.Rules;

namespace Evently.ArchitectureTests.Abstractions;

internal static  class TestResultExtionsions
{
    internal static void ShouldBeSuccessFul(this TestResult testResult)
    {
        testResult.FailingTypes?.Should().BeEmpty();
    }
}
