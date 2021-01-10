using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using NBitcoin;
using Proto;
using StarFinder.Messages;
using StarFinder.Utils;

namespace StarFinder.Actors
{
    public class WorkerActor : IActor
    {
        private readonly IMyLogger _logger;
        private long NumKeysTried;

        public WorkerActor(IMyLogger logger)
        {
            _logger = logger.For<WorkerActor>();
            NumKeysTried = 0;
        }


        public Task ReceiveAsync(IContext context)
        {
            _logger.Info($"got msg {context.Message}");
            switch (context.Message)
            {
                case Started _:
                    _logger.Warning($"Worker {context.Self} restarting...");
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
                    StartCrack(context);
                    break;
                case RequestNumKeysTried _:
                    context.Respond(new ResponseNumKeysTried(NumKeysTried));
                    break;

            }
            return Task.CompletedTask;
        }

        private void StartCrack(IContext context)
        {
            Key privateKey = new Key(); // generate a random private key
            BitcoinSecret bitcoinSecret = privateKey.GetWif(Network.Main);
            string address = bitcoinSecret.GetAddress(ScriptPubKeyType.Legacy).ToString();
            if (BTCAddressLoader.AddressBook.Contains(address))
            {
                //bingo!
                Config.AddKey(bitcoinSecret.ToString(), address);
                _logger.Info("Found a key!!!");
            }
            else
            {
                //keep working
                context.Send(context.Self!, new StartCracking());
            }

            NumKeysTried++;
        }
    }
}