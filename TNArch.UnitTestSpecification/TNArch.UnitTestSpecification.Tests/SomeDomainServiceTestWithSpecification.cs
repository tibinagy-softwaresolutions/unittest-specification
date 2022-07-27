using FluentAssertions;
using Moq;
using NUnit.Framework;
using TNArch.UnitTestSpecification.Core;

namespace TNArch.UnitTestSpecification.Tests
{
    public class SomeDomainServiceTestWithSpecification
    {
        [Test]
        public void DoSomethingAsync_WhenSomeTestCase_SomeResultRetuned()
        {
            var someValues = new SomeOptions { SomeOption = "Option1" };
            var someData = new[] { new SomeData { Value = "D1" } };

            Specification.ForService<SomeDomainService>()
               .GivenOptions(someValues)
               .GivenMock<ISomeRepository>(m => m.Setup(x => x.GetSomeData(someValues.SomeOption, "input1")).ReturnsAsync(someData))
               .GivenLogging()
               .GivenPartialMock(m => m.Setup(x => x.DoSomethingElse(someData, "input1")).Returns("Result1"))
               .WhenExecute(sut => sut.DoSomethingAsync("input1").Result)
               .ThenResult(r => r.Should().Be("Result1"))
               .ThenPartialMock(m => m.VerifyCalledOnce(x => x.DoSomethingElse(someData, "input1")))
               .ThenInfoLog("Doing sometthing for TNArch.UnitTestSpecification.Tests.SomeData[]");
        }        
    }
}