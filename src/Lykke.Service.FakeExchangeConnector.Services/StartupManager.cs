using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;

namespace Lykke.Service.FakeExchangeConnector.Services
{
    // NOTE: Sometimes, startup process which is expressed explicitly is not just better, 
    // but the only way. If this is your case, use this class to manage startup.
    // For example, sometimes some state should be restored before any periodical handler will be started, 
    // or any incoming message will be processed and so on.
    // Do not forget to remove As<IStartable>() and AutoActivate() from DI registartions of services, 
    // which you want to startup explicitly.

    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;
        
        private readonly IExchangeCache _exchangeCache;

        private readonly FakeExchangeConnectorSettings _fakeExchangeConnectorSettings; 

        public StartupManager(ILog log,
            IExchangeCache exchangeCache,
            FakeExchangeConnectorSettings fakeExchangeConnectorSettings)
        {
            _log = log;
            
            _exchangeCache = exchangeCache;

            _fakeExchangeConnectorSettings = fakeExchangeConnectorSettings;
        }

        public async Task StartAsync()
        {
            // TODO: Implement your startup logic here. Good idea is to log every step

            var exchangeSettings = _fakeExchangeConnectorSettings.ExchangesConfig.Select(ex => new Exchange(ex.Name)
            {
                Instruments =
                    ex.Instruments?.Select(x => new Instrument(ex.Name, x)).ToList() ?? new List<Instrument>(),
                Accounts = ex.Accounts ?? new AccountBalance[0],
                Positions = ex.Positions?.Select(x => new Position
                {
                    Symbol = x.Symbol,
                    PositionVolume = x.PositionVolume
                }).ToList() ?? new List<Position>(),
                StreamingSupport = new StreamingSupport(ex.StreamingSupport.OrderBooks, ex.StreamingSupport.Orders),
                AcceptOrder = ex.AcceptOrder,
                PushEventToRabbit = ex.PushEventToRabbit
            });
            
            _exchangeCache.Initialize(exchangeSettings);
            
            await Task.CompletedTask;
        }
    }
}
