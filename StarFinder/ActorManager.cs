using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Proto;
using StarFinder.Actors;
using StarFinder.Messages;
using StarFinder.Utils;

namespace StarFinder
{
    public class ActorManager : IActor
    {
        private readonly IMyLogger _logger;
        private List<PID> _allWorkers;
        private long _totalKeysTried;
        private CancellationTokenSource _cts;

        public ActorManager(IMyLogger logger)
        {
            _logger = logger.For<ActorManager>();
            _allWorkers = new List<PID>();
            _totalKeysTried = 0;
        }


        public Task ReceiveAsync(IContext context)
        {
            _logger.Info($"got msg {context.Message}");
            switch (context.Message)
            {
                case Started _:
                    _logger.Info("!!!!!!!!!!!!!");
                    break;
                case SpawnWorker msg:
                    for (int i = 0; i < msg.Amount; i++)
                        CreateWorkers(context);
                    break;
                case RequestWorkerCount _:
                    _logger.Info($"Total workers: {GetWorkerCount()}");
                    break;
                case ShowStatistics msg:
                    foreach (PID pid in _allWorkers)
                    {
                       // _scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(5), msg.Duration, pid,
                       //     new RequestNumKeysTried(), out _cts);
                    }
                    break;
                case ResponseNumKeysTried msg:
                    _totalKeysTried += msg.Num;
                    break;
                case ShowTotalKeysTried _:
                    _logger.Info($"Total number of keys tried: {_totalKeysTried}");
                    break;

            }
            return Task.CompletedTask;
        }

        private int GetWorkerCount()
        {
            return _allWorkers.Count;
        }

        private void CreateWorkers(IContext context)
        {
            int id = _allWorkers.Count;
            Props props = Props.FromProducer(() => new WorkerActor(_logger));
            PID pid = context.SpawnNamed(props, "Worker_" + id);
            _allWorkers.Add(pid);
        }

    }
}