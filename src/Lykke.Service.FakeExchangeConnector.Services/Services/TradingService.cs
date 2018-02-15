using System;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Service.FakeExchangeConnector.Core.Services;

namespace Lykke.Service.FakeExchangeConnector.Services.Services
{
    public class TradingService : ITradingService
    {
        private readonly IExchangeCache _exchangeCache;

        private readonly IExecutionReportPublisher _executionReportPublisher;

        public TradingService(IExchangeCache exchangeCache,
            IExecutionReportPublisher executionReportPublisher)
        {
            _exchangeCache = exchangeCache;

            _executionReportPublisher = executionReportPublisher;
        }
        
        public async Task<ExecutionReport> CreateOrder(string exchangeName, string instrument, TradeType tradeType, 
            decimal price, decimal volume)
        {
            var executionReport = new ExecutionReport
            {
                Instrument = new Instrument(exchangeName, instrument),
                Time = DateTime.UtcNow,
                Price = price,
                Volume = volume,
                Type = tradeType,
                Fee = 0,
                ExchangeOrderId = Guid.NewGuid().ToString(),
                ExecutionStatus = OrderExecutionStatus.Fill
            };
            
            var exchange = _exchangeCache.UpdatePosition(exchangeName, instrument, tradeType, volume);
            
            //push to rabbit
            await _executionReportPublisher.Publish(executionReport);
            
            return executionReport;
        }

        public bool? GetAcceptOrder(string exchangeName)
        {
            return _exchangeCache.Get(exchangeName)?.AcceptOrder;
        }

        public bool? SetAcceptOrder(string exchangeName, bool acceptOrder)
        {
            var exchange = _exchangeCache.Get(exchangeName);

            if (exchange == null)
                return null;

            exchange.AcceptOrder = acceptOrder;
            
            _exchangeCache.Set(exchange);

            return acceptOrder;
        }

        public bool? GetPushEventToRabbit(string exchangeName)
        {
            return _exchangeCache.Get(exchangeName)?.PushEventToRabbit;
        }

        public bool? SetPushEventToRabbit(string exchangeName, bool pushEventToRabbit)
        {
            var exchange = _exchangeCache.Get(exchangeName);

            if (exchange == null)
                return null;

            exchange.PushEventToRabbit = pushEventToRabbit;
            
            _exchangeCache.Set(exchange);

            return pushEventToRabbit;
        }
    }
}
