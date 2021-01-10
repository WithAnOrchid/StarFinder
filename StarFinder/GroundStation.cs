using System;
using StarFinder.Utils;
using System.Threading;
using System.Threading.Tasks;
using Proto;
using StarFinder.Messages;

namespace StarFinder
{
    public class GroundStation
    {
        private readonly IMyLogger _logger;
        private ActorSystem _actorSystem;
        private PID _actorManager;
        public GroundStation()
        {
            Config.ReloadConfig();

            IMyLogger myLogger = new MyLogger();
            _logger = myLogger.For<GroundStation>();

            _actorSystem = new ActorSystem();

            _logger.Info("Started");
        }

        public async void Run()
        {
            Props props = Props.FromProducer(() => new ActorManager(_logger));
            _actorManager = _actorSystem.Root.SpawnNamed(props, "ActorManager");

            Task<int> task = BTCAddressLoader.LoadAddressesAsync(_logger);
            int count = await task;
            _logger.Info($"Successfully loaded {count} addresses");

            //_actorSystem.Root.Send(_actorManager, new SpawnWorker(1));
            //_actorSystem.Root.Send(_actorManager, new ShowStatistics(TimeSpan.FromSeconds(10)));

            _ = Task.Run(() =>
              {
                  while (true)
                  {
                      Task.Delay(5000).Wait();
                      //_actorSystem.Root.Send(_actorManager, new RequestWorkerCount());
                  }
              });
        }
    }
}