using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TNArch.UnitTestSpecification.Core;

namespace TNArch.UnitTestSpecification.Tests
{
    public class SomeDomainServiceTests
    {
        private readonly Mock<ILogger<SomeDomainService>> _loggerMock = new();
        private readonly Mock<ISomeRepository> _repositoryMock = new();
        private readonly Mock<IOptions<SomeOptions>> _optionsMock = new();

        private readonly SomeOptions _options;

        public SomeDomainServiceTests()
        {
            _options = new SomeOptions { SomeOption = "Option1" };

            _optionsMock.SetupGet(x => x.Value).Returns(_options);
        }

        [Test]
        public async Task DoSomethingAsync_WhenSomeTestCase_SomeResultRetunedAsync()
        {
            //Arange
            var someData = new[] { new SomeData { Value = "D1" } };

            _repositoryMock.Setup(x => x.GetSomeData(_options.SomeOption, "input1")).ReturnsAsync(someData);

            var someServiceMock = new Mock<SomeDomainService>(_loggerMock.Object, _repositoryMock.Object, _optionsMock.Object) { CallBase = true };

            someServiceMock.Setup(x => x.DoSomethingElse(someData, "input1")).Returns("Result1");

            //Act
            var result = await someServiceMock.Object.DoSomethingAsync("input1");

            //Assert
            result.Should().Be("Result1");

            someServiceMock.VerifyCalledOnce(x => x.DoSomethingElse(someData, "input1"));

            _loggerMock.Verify(logger => logger.Log
               (
                  LogLevel.Information,
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((object o, Type t) => o.ToString()!.Contains("Doing sometthing for")),
                  It.IsAny<Exception>(),
                  (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
               Times.Once
            );
        }

    }
}