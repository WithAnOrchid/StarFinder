using StarFinder.Utils;
using System.Threading;
using System.Threading.Tasks;

namespace StarFinder
{
    public class GroundStation
    {
        private readonly IMyLogger _logger;

        public GroundStation()
        {
            Config.ReloadConfig();
            IMyLogger myLogger = new MyLogger();
            _logger = myLogger.For<GroundStation>();
            _logger.Info("Started");
        }

        public async void Run()
        {
            Task<int> task = BTCAddressLoader.LoadAddressesAsync(_logger);
            int count = await task;
            _logger.Info($"Successfully loaded {count} addresses");
        }
    }
}