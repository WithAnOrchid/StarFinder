using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using Proto;
using StarFinder.Messages;
using StarFinder.Utils;

namespace StarFinder.Actors
{
    public class WorkerActor : IActor
    {
        private readonly IMyLogger _logger;

        public WorkerActor(IMyLogger logger)
        {
            _logger = logger.For<WorkerActor>();
        }


        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started _:
                    _logger.Info($"Worker {context.Self} started");
                    context.Send(context.Self!, new StartCracking());
                    break;
                case Stopping _:
                    _logger.Error($"Worker {context.Self} stopping...");
                    break;
                case Stopped _:
                    _logger.Error($"Worker {context.Self} stopped");
                    break;
                case Restarting _:
                    _logger.Warning($"Worker {context.Self} restarting...");
                    break;

                case StartCracking _:
                    StartCrack();
                    break;

            }
            return Task.CompletedTask;
        }

        private void StartCrack()
        {

        }
    }
}