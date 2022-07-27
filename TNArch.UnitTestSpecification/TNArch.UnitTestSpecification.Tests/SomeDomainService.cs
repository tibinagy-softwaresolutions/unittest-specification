using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace TNArch.UnitTestSpecification.Tests
{
    public class SomeDomainService
    {
        private readonly ILogger<SomeDomainService> _logger;
        private readonly ISomeRepository _repository;
        private readonly IOptions<SomeOptions> _options;

        public SomeDomainService(ILogger<SomeDomainService> logger, ISomeRepository repository, IOptions<SomeOptions> options)
        {
            _logger = logger;
            _repository = repository;
            _options = options;
        }

        public async Task<string> DoSomethingAsync(string someInput)
        {
            var someData = await _repository.GetSomeData(_options.Value.SomeOption, someInput);

            _logger.LogInformation($"Doing sometthing for {someData}");

            return DoSomethingElse(someData, someInput);
        }

        public virtual string DoSomethingElse(SomeData[] someData, string someInput)
        {
            return someInput + string.Join(", ", someData.Select(d => d.Value));
        }
    }

    public interface ISomeRepository
    {
        public Task<SomeData[]> GetSomeData(string someOption, string input);
    }

    public class SomeOptions
    {
        public string SomeOption { get; set; }
    }


    public class SomeData
    {
        public string Value { get; set; }
    }
}