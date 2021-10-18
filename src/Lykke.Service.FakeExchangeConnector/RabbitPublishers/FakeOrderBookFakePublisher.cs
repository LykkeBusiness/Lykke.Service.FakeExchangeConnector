using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Snow.Common.Correlation.RabbitMq;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.FakeExchangeConnector.RabbitPublishers
{
    public sealed class FakeOrderBookFakePublisher : RabbitMqPublisher<OrderBook>, IFakeOrderBookPublisher
    {
        public FakeOrderBookFakePublisher(
            RabbitMqCorrelationManager correlationManager,
            ILoggerFactory loggerFactory,
            string connectionString, 
            string exchangeName, 
            bool enabled, 
            ILog log,
            bool durable = false,
            string messageFormat = null) 
            : base(correlationManager, loggerFactory, connectionString, exchangeName, enabled, log, durable, messageFormat)
        {
            
        }
        
        
    }
}
