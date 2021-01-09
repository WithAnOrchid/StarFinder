using System.Collections.Concurrent;
using Proto;
using StarFinder.Utils;

namespace StarFinder
{
    public class ActorManager
    {
        private readonly IMyLogger _logger;
        private ActorSystem _actorSystem;

        private ConcurrentBag<PID> _allWorkers;

        public int GetWorkerCount()
        {
            return _allWorkers.Count;
        }

        public ActorManager(IMyLogger logger)
        {
            _logger = logger.For<ActorManager>();
            _allWorkers = new ConcurrentBag<PID>();
            _actorSystem = new ActorSystem();
        }

        public void CreateWorkers()
        {

        }
    }
}