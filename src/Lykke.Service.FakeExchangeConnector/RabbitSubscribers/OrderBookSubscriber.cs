using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Snow.Common.Correlation.RabbitMq;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.FakeExchangeConnector.RabbitSubscribers
{
    public class OrderBookSubscriber : RabbitMqSubscriber<OrderBook>
    {
        public OrderBookSubscriber(
            RabbitMqCorrelationManager correlationManager,
            ILoggerFactory loggerFactory,
            ILog log,
            string connectionString,
            string exchangeName,
            string queueName, 
            bool isDurable,
            string messageFormat)
            : base(correlationManager, loggerFactory, log, connectionString, exchangeName, queueName, isDurable, messageFormat)
        {
            
        }
    }
}
